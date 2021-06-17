using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalR.Client.Core;
using SignalR.Client.Core.Management.Interface;
using SignalR.Model.Db;

namespace SignalR.Client
{
    public class Worker : BackgroundService
    {
        private ILogger<Worker> Logger { get; }
        private IHubConnectionManager HubConnectionManager { get; }

        public Worker(ILogger<Worker> logger, 
            IHubConnectionManager hubConnectionManager)
        {
            Logger = logger;
            HubConnectionManager = hubConnectionManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                HubConnectionManager.RegisterEndPoints(stoppingToken);
                await HubConnectionManager.StartAsync(stoppingToken);
                Console.WriteLine("Connection started");

                await HubConnectionManager.JoinGroupAsync(stoppingToken);
                Console.WriteLine("Joined Devices group");

                while (!stoppingToken.IsCancellationRequested)
                {
                    Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }

                var cancellationToken = new CancellationToken();
                await HubConnectionManager.LeaveGroupAsync(cancellationToken);
                await HubConnectionManager.StopAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Logger.LogError(ex, "An error occurred while running the application");
            }
        }
    }
}
