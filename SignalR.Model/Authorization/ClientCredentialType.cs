using SignalR.Model.Interfaces.AuthorizationApi.CredentialType;

namespace SignalR.Model.Authorization
{
    public class ClientCredentialType : IClientCredentialType
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
