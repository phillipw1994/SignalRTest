using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SignalR.Client.ApiClient.Authorization.Interfaces;
using SignalR.Model.Authorization;
using SignalR.Model.Configuration;
using SignalR.Model.Exceptions;
using SignalR.Model.Interfaces.AuthorizationApi;

namespace SignalR.Client.ApiClient.Authorization.Managers
{
    public class SystemAuthenticatorAsync : ISystemAuthenticatorAsync
    {
        private IConnectionInfo ConnectionInfo { get; }

        public SystemAuthenticatorAsync(IConfigurationSettings configurationSettings)
        {
            ConnectionInfo = configurationSettings.ConnectionInfo;
        }

        public async Task<IEnumerable<ISystemResponse>> GetSystemsAsync(IApiCredentials apiCredentials)
        {
            try
            {
                IEnumerable<ISystemResponse> dataObjects;

                using var client = new HttpClient()
                {
                    BaseAddress =
                        new Uri($"{ConnectionInfo.AuthApi}Account/systems"),
                    Timeout = TimeSpan.FromSeconds(15)
                };

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiCredentials.AccessToken);
                var response = await client.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    dataObjects = await response.Content.ReadAsAsync<IEnumerable<SystemResponse>>();
                }
                else
                {
                    throw new HttpRequestException(
                        $"Status Code: {response.StatusCode}{Environment.NewLine}Reason Phrase: {response.ReasonPhrase}");
                }
                return dataObjects ?? new List<ISystemResponse>();
            }
            catch (Exception ex)
            {
                var webEx = (WebException)ex;
                //Logger.Log(LogLevel.Error, @"Login request is bad.", ex);
                throw new Exception($"Exception Occurred{Environment.NewLine}{webEx.Message}");
            }
        }

        public async Task<IAuthResponse> LoginRequestAsync(IApiCredentials apiCredentials)
        {
            AuthResponse authResponse;

            using var client = new HttpClient
            {
                BaseAddress =
                    new Uri(ConnectionInfo.AuthApi),
                Timeout = TimeSpan.FromSeconds(15)
            };

            #region build request

            List<KeyValuePair<string, string>> formData;
            if (apiCredentials.UserCredential != null)
            {
                //Create our form data as key value pairs
                formData = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "password"),
                    new("username", apiCredentials.UserCredential.Email),
                    new("password", apiCredentials.UserCredential.Password),
                    new("scope", "offline_access roles profile email paws_app_api device"),
                    new("client_id", "paws_application")
                };
                if (apiCredentials.SystemUid != null && apiCredentials.SystemUid != Guid.Empty)
                {
                    formData.Add(new KeyValuePair<string, string>("system_uid", apiCredentials.SystemUid.ToString()));
                    formData.Add(new KeyValuePair<string, string>("device_uid", apiCredentials.DeviceUid.ToString()));
                    formData.Add(new KeyValuePair<string, string>("device_name", apiCredentials.DeviceName));
                }
            }
            else if (apiCredentials.ClientCredential != null)
            {
                formData = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "client_credentials"),
                    new("client_id", apiCredentials.ClientCredential.ClientId),
                    new("client_secret", apiCredentials.ClientCredential.ClientSecret)
                };
            }
            else
                throw new Exception("Login Method not supported");





            //Create our web request
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("connect/token", UriKind.Relative),
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(formData)
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            #endregion

            #region validate response

            var bearerResult = await client.SendAsync(request);
            if (bearerResult.IsSuccessStatusCode)
            {
                //Get bearer data
                authResponse = await bearerResult.Content.ReadAsAsync<AuthResponse>();

                //Create user fields for User object
                if (!double.TryParse(authResponse.ExpiresIn.ToString(), out var expTimeDouble))
                    throw new Exception("Expiry Time is in incorrect format");

                authResponse.ExpirationDateTime = DateTime.Now.AddSeconds(expTimeDouble);
            }
            else
            {
                throw new AuthenticationApiException(
                    $"-Login failed with username and password supplied{Environment.NewLine}Please try again", bearerResult.StatusCode, bearerResult.ReasonPhrase);
            }
            #endregion

            return authResponse;
        }

        public async Task<IAuthResponse> GetUserUidAsync(IApiUser user)
        {
            using var client = new HttpClient { BaseAddress = new Uri($"{ConnectionInfo.AuthApi}account/user"), Timeout = TimeSpan.FromSeconds(15) };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await client.GetAsync("");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"-Login failed with username and password supplied{Environment.NewLine}Please try again");

            //ToDo: Once api updated use code below to return data
            //var apiUser = await response.Content.ReadAsAsync<AuthApiUser>();

            var stringData = await response.Content.ReadAsStringAsync();
            var bearerJson = JObject.Parse(stringData);
            var bearerUid = bearerJson["uid"]?.ToString().Trim('"');

            return new AuthResponse
            {
                Uid = bearerUid
            };
        }

        public async Task<IAuthResponse> RefreshRequestAsync(IApiUser user)
        {
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
                //Create our form data as key value pairs
                var formData = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "refresh_token"),
                    new("refresh_token", user.RefreshToken),
                    new("scope", "offline_access roles profile email"),
                    new("client_id", "paws_application")
                };

                //Create our web request
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{ConnectionInfo.AuthApi}connect/token"),
                    Method = HttpMethod.Post,
                    Content = new FormUrlEncodedContent(formData)
                };
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var responseTime = DateTime.Now;
                var bearerResult = await client.SendAsync(request);

                bearerResult.EnsureSuccessStatusCode();

                var tokenResponse = await bearerResult.Content.ReadAsAsync<ApiTokenResponse>();
                var bearerToken = tokenResponse.access_token;
                var expTime = tokenResponse.expires_in;

                if (!double.TryParse(expTime.ToString(), out var expTimeDouble))
                    throw new Exception("Expiry Time is in incorrect format");

                var expDate = responseTime.AddSeconds(expTimeDouble);
                return new AuthResponse { AccessToken = bearerToken, ExpirationDateTime = expDate };
            }
            catch (WebException webEx)
            {
                if (!webEx.Message.Contains("(400) Bad Request"))
                    throw new Exception(
                        $"An error occurred trying to authorize the user{Environment.NewLine}Please check this device can access the authentication api.{Environment.NewLine}If problem persists please contact R.A.C.E Services Ltd",
                        webEx);

                string exData;
                using (var sr = new StreamReader(webEx.Response.GetResponseStream()))
                {
                    exData = await sr.ReadToEndAsync();
                }
                var exJson = JObject.Parse(exData);
                var exDescription = exJson["error_description"]?.ToString().Trim('"');
                switch (exDescription)
                {
                    case "The specified refresh token is no longer valid.":
                        throw new RefreshTokenInvalidException("The specified refresh token is no longer valid.");
                    case "The specified refresh token is invalid.":
                        throw new RefreshTokenInvalidException("The specified refresh token is no longer valid.");
                    default:
                        throw new Exception(
                            $"An error occurred trying to authorize the user{Environment.NewLine}{exDescription}", webEx);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"-There was a system error while trying to process the request{Environment.NewLine}Please try again", ex);
            }
        }
    }
}