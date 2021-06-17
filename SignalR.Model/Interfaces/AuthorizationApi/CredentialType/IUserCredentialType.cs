namespace SignalR.Model.Interfaces.AuthorizationApi.CredentialType
{
    public interface IUserCredentialType
    {
        string Email { get; set; }
        string Password { get; set; }
    }
}
