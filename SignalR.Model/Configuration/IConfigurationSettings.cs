using SignalR.Model.Authorization;

namespace SignalR.Model.Configuration
{
    public interface IConfigurationSettings
    {
        BaseConnectionInfo ConnectionInfo { get; }
        string InfusionDataLocation { get; }
        int? BatchSyncSize { get; }
        string ToBePickedStatus { get; }
        string PickedStatus { get; }
        string LocationsToBePicked { get; }
        string ClientId { get; }
        string ClientSecret { get; }

        string ConnectionString(string connectionStringName);
    }
}
