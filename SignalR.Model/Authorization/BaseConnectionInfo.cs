#region Usings

#endregion

using SignalR.Model.Interfaces.AuthorizationApi;

namespace SignalR.Model.Authorization
{
    public class BaseConnectionInfo : IConnectionInfo
    {
        public string AuthApi { get; set; }
        public bool AuthRequired { get; set; }
        public string DeviceApi { get; set; }
        public bool RetryConnection { get; set; }
        public int MaxRetries { get; set; }
        public int PageSize { get; set; }
        public int? ConnectionTimeout { get; set; }
    }
}
