using System.IO;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SignalR.Client.ApiClient.Authorization.Interfaces;
using SignalR.Client.ApiClient.Authorization.Managers;
using SignalR.Client.Core;
using SignalR.Client.Core.Management;
using SignalR.Client.Core.Management.Interface;
using SignalR.Database;
using SignalR.Database.Helpers;
using SignalR.Model.Configuration;
using SignalR.Model.Db;

namespace SignalR.Client.Ioc
{
    public class Registrar
    {
        public static void RegisterTypes(IConfiguration configuration, ContainerBuilder builder)
        {
            //Create ProgramData folders
            if (!Directory.Exists("C:\\ProgramData\\SignalRTest\\"))
                Directory.CreateDirectory("C:\\ProgramData\\SignalRTest\\");

            IConfigurationSettings configurationSettings = new JsonConfigurationSettings(configuration);

            using ISignalRDbContext context = new SignalRDbContext(DatabaseHelpers
                .CreateOptionsBuilder<SignalRDbContext>(configurationSettings.ConnectionString("DefaultConnection"))
                .Options);

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            // Register Configuration
            builder.Register(c => new JsonConfigurationSettings(configuration))
                .As<IConfigurationSettings>()
                .InstancePerLifetimeScope();

            //Register Type
            builder.RegisterType<SystemAuthenticatorAsync>()
                .As<ISystemAuthenticatorAsync>()
                .InstancePerLifetimeScope();

            //Register Type
            builder.RegisterType<ClientCredentialManagerAsync<ClientCredential>>()
                .As<IClientCredentialManagerAsync<ClientCredential>>()
                .InstancePerDependency();


            builder.Register(c => new HubConnectionBuilder()
                    .WithUrl(configurationSettings.DeviceHub,
                        options =>
                    {
                        //options.AccessTokenProvider = async () =>
                        //{
                        //    await ClientCredentialManager.CheckTokenExpirationAsync(CancellationToken.None);
                        //    return await ClientCredentialManager.GetAccessTokenAsync(CancellationToken.None);
                        //    //Task.FromResult(_myAccessToken)
                        //};
                    })
                    .WithAutomaticReconnect())
                .As<IHubConnectionBuilder>()
                .InstancePerLifetimeScope();

            builder.RegisterType<HubConnectionManager>().As<IHubConnectionManager>()
                .InstancePerDependency();
        }
    }
}
