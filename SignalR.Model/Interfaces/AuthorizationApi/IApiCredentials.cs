using System;
using SignalR.Model.Interfaces.AuthorizationApi.CredentialType;

namespace SignalR.Model.Interfaces.AuthorizationApi
{
    public interface IApiCredentials
    {
        IClientCredentialType ClientCredential { get; set; }
        IUserCredentialType UserCredential { get; set; }
        string AccessToken { get; set; }
        Guid? SystemUid { get; set; }
        Guid DeviceUid { get; set; }
        string DeviceName { get; set; }
    }
}
