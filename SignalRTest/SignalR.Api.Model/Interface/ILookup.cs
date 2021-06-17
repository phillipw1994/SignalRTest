namespace SignalR.Api.Model.Interface
{
    public interface ILookup : IKeyId
    {
        string Name { get; set; }
        string FriendlyName { get; set; }
        string Description { get; set; }
    }
}