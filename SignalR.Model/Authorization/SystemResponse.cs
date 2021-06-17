using SignalR.Model.Interfaces.AuthorizationApi;

namespace SignalR.Model.Authorization
{
    public class SystemResponse : ISystemResponse
    {
        public string Uid { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
    }
}
