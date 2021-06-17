using System;
using Newtonsoft.Json;

namespace SignalR.Model.Interfaces.AuthorizationApi
{
    public interface IAuthResponse
    {
        [JsonProperty("token_type")]
        string TokenType { get; set; }

        [JsonProperty("access_token")]
        string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        string RefreshToken { get; set; }

        [JsonProperty("id_token")]
        string IdToken { get; set; }

        [JsonProperty("expirationDateTime")]
        DateTime ExpirationDateTime { get; set; }

        [JsonProperty("uid")]
        string Uid { get; set; }
    }
}
