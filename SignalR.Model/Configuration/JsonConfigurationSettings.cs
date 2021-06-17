using Microsoft.Extensions.Configuration;
using SignalR.Model.Authorization;

namespace SignalR.Model.Configuration
{
    public class JsonConfigurationSettings : IConfigurationSettings
    {
        #region constructors
        public JsonConfigurationSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region private members
        private IConfiguration Configuration { get; }

        private BaseConnectionInfo _connectionInfo;
        private int? _batchSyncSize;
        private string _infusionDataLocation;
        private string _toBePickedStatus;
        private string _pickedStatus;
        private string _locationsToBePicked;
        private string _clientId;
        private string _clientSecret;
        #endregion

        public BaseConnectionInfo ConnectionInfo => _connectionInfo ??= Configuration.GetSection("ConnectionInfo").Get<BaseConnectionInfo>();
        public string InfusionDataLocation => _infusionDataLocation ??= Configuration.GetSection("InfusionDataLocation").Get<string>();
        public int? BatchSyncSize => _batchSyncSize ??= Configuration.GetSection("BatchSyncSize").Get<int>();
        public string ToBePickedStatus => _toBePickedStatus ??= Configuration.GetSection("ToBePickedStatus").Get<string>();
        public string PickedStatus => _pickedStatus ??= Configuration.GetSection("PickedStatus").Get<string>();
        public string LocationsToBePicked => _locationsToBePicked ??= Configuration.GetSection("LocationsToBePicked").Get<string>();
        public string ClientId => _clientId ??= Configuration.GetSection("ClientId").Get<string>();
        public string ClientSecret => _clientSecret ??= Configuration.GetSection("ClientSecret").Get<string>();

        public string ConnectionString(string connectionStringName)
        {
            return Configuration.GetConnectionString(connectionStringName);
        }
    }
}
