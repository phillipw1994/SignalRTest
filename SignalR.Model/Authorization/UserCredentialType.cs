using SignalR.Model.Interfaces.AuthorizationApi.CredentialType;

namespace SignalR.Model.Authorization
{
    public class UserCredentialType : IUserCredentialType
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
