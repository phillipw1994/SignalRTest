using System.Threading.Tasks;
using SignalR.Model.Interfaces.AuthorizationApi;

namespace SignalR.Client.ApiClient.Authorization.Interfaces
{
    public interface ISystemAuthenticatorAsync
    {
        Task<IAuthResponse> LoginRequestAsync(IApiCredentials apiCredentials);
        Task<IAuthResponse> GetUserUidAsync(IApiUser user);
        Task<IAuthResponse> RefreshRequestAsync(IApiUser user);
    }
}
