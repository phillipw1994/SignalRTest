using System;
using System.Threading;
using System.Threading.Tasks;
using SignalR.Model.Interfaces;
using SignalR.Model.Interfaces.AuthorizationApi;

namespace SignalR.Client.Core.Management.Interface
{
    public interface IClientCredentialManagerAsync<T> where T: class, IClientCredential
    {
        Task<T> LoginAsync(IApiCredentials apiCredentials, CancellationToken cancellationToken);
        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken);
        Task CheckTokenExpirationAsync(CancellationToken cancellationToken);
        Task<bool> HasSystemInTokenAsync(string userAccessToken, CancellationToken cancellationToken);
        Task<Guid?> GetSystemFromTokenAsync(string userAccessToken, CancellationToken cancellationToken);
    }
}
