using System;

namespace SignalR.Model.Interfaces
{
    public interface IClientCredential : ILocalDbEntity
    {
        string AccessToken { get; set; }
        DateTime ExpiryDateTime { get; set; }
    }
}
