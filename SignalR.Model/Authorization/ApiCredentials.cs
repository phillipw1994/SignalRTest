using System;
using SignalR.Model.Interfaces.AuthorizationApi;
using SignalR.Model.Interfaces.AuthorizationApi.CredentialType;

namespace SignalR.Model.Authorization
{
    public class ApiCredentials : IApiCredentials
    {
        public IClientCredentialType ClientCredential { get; set; }
        public IUserCredentialType UserCredential { get; set; }
        public string AccessToken { get; set; }
        public Guid? SystemUid { get; set; }
        public Guid DeviceUid { get; set; }
        public string DeviceName { get; set; }
    }
}
