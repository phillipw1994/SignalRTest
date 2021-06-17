namespace SignalR.Model.Interfaces.AuthorizationApi.CredentialType
{
    public interface IClientCredentialType
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
    }
}
