namespace SignalR.Model.Interfaces.AuthorizationApi
{
    public interface ISystemResponse
    {
        string Uid { get; set; }
        string Name { get; set; }
        string FriendlyName { get; set; }
        string Description { get; set; }
        bool Active { get; set; }
        bool Deleted { get; set; }
    }
}
