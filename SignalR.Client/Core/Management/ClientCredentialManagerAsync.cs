using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SignalR.Client.ApiClient.Authorization.Interfaces;
using SignalR.Client.Core.Management.Interface;
using SignalR.Database.Interfaces.Repository;
using SignalR.Database.Repositories;
using SignalR.Model.Authorization;
using SignalR.Model.Configuration;
using SignalR.Model.Interfaces;
using SignalR.Model.Interfaces.AuthorizationApi;

namespace SignalR.Client.Core.Management
{
    public class ClientCredentialManagerAsync<T> : IClientCredentialManagerAsync<T> where T : class, IClientCredential, ILocalDbEntity, new()
    {
        #region constructors

        public ClientCredentialManagerAsync(ISystemAuthenticatorAsync authenticator, IConfigurationSettings configurationSettings)
        {
            Authenticator = authenticator;
            ConfigurationSettings = configurationSettings;
            ClientCredentialRepository = new LocalDbClientCredentialRepository<T>(configurationSettings.ConnectionString("DefaultConnection"));
        }

        #endregion

        #region private members

        private ISystemAuthenticatorAsync Authenticator { get; }
        private ILocalDbClientCredentialRepository<T> ClientCredentialRepository { get; }
        private IConfigurationSettings ConfigurationSettings { get; }

        #endregion

        public async Task<T> LoginAsync(IApiCredentials apiCredentials, CancellationToken cancellationToken)
        {
            var clientCredentials = await ClientCredentialRepository.GetListAsync();
            if (!clientCredentials.Any())
            {
                var authResponse = await Authenticator.LoginRequestAsync(apiCredentials);
                if (authResponse == null) throw new Exception("Failed to login");
                var clientCredential = new T
                {
                    Uid = Guid.NewGuid(),
                    AccessToken = authResponse.AccessToken,
                    ExpiryDateTime = authResponse.ExpirationDateTime
                };
                await ClientCredentialRepository.AddAsync(clientCredential, cancellationToken);
                return await Task.FromResult(clientCredential);
            }
            else
            {
                var clientCredential = clientCredentials.First();
                var authResponse = await Authenticator.LoginRequestAsync(apiCredentials);
                if (authResponse == null) throw new Exception("Failed to login");
                clientCredential.AccessToken = authResponse.AccessToken;
                clientCredential.ExpiryDateTime = authResponse.ExpirationDateTime;
                await ClientCredentialRepository.UpdateAsync(clientCredential, cancellationToken);
                return await Task.FromResult(clientCredential);
            }
        }

        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            var clientCredentials = await ClientCredentialRepository.GetListAsync();
            if (!clientCredentials.Any())
                throw new Exception("Failed to get client credentials");
            var clientCredential = clientCredentials.First();
            return await Task.FromResult(clientCredential.AccessToken);
        }

        public async Task CheckTokenExpirationAsync(CancellationToken cancellationToken)
        {
            var clientCredentials = await ClientCredentialRepository.GetListAsync();
            if (!clientCredentials.Any())
            {
                //ToDO this should not be here
                IApiCredentials apiCredentials = new ApiCredentials
                {
                    ClientCredential = new ClientCredentialType
                        { ClientId = ConfigurationSettings.ClientId, ClientSecret = ConfigurationSettings.ClientSecret },
                    DeviceUid = Guid.NewGuid()
                };
                await LoginAsync(apiCredentials, cancellationToken);
                return;
            }

            var clientCredential = clientCredentials.First();

            if (clientCredential.ExpiryDateTime <= DateTime.Now)
            {
                //ToDO this should not be here
                IApiCredentials apiCredentials = new ApiCredentials
                {
                    ClientCredential = new ClientCredentialType
                        {ClientId = ConfigurationSettings.ClientId, ClientSecret = ConfigurationSettings.ClientSecret},
                    DeviceUid = Guid.NewGuid()
                };
                await LoginAsync(apiCredentials, cancellationToken);
            }
        }

        public Task<bool> HasSystemInTokenAsync(string userAccessToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Guid?> GetSystemFromTokenAsync(string userAccessToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
