using System;
using Newtonsoft.Json;
using SignalR.Model.Interfaces.AuthorizationApi;

namespace SignalR.Model.Authorization
{
    public class AuthResponse : IAuthResponse
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("expirationDateTime")]
        public DateTime ExpirationDateTime { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }
    }
}
