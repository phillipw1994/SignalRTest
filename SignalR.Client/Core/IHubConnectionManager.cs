using System.Threading;
using System.Threading.Tasks;

namespace SignalR.Client.Core
{
    public interface IHubConnectionManager
    {
        void RegisterEndPoints(CancellationToken stoppingToken);
        Task StartAsync(CancellationToken stoppingToken);
        Task JoinGroupAsync(CancellationToken stoppingToken);
        Task LeaveGroupAsync(CancellationToken stoppingToken);
        Task StopAsync(CancellationToken stoppingToken);
    }
}