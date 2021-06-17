namespace SignalR.Model.Interfaces.AuthorizationApi
{
    public interface IConnectionInfo
    {
        string AuthApi { get; set; }
        bool AuthRequired { get; set; }
        string DeviceApi { get; set; }
        bool RetryConnection { get; set; }
        int MaxRetries { get; set; }
        int PageSize { get; set; }
        int? ConnectionTimeout { get; set; }
    }
}
