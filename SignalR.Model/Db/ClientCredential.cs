using System;
using System.ComponentModel.DataAnnotations;
using SignalR.Model.Interfaces;

namespace SignalR.Model.Db
{
    public class ClientCredential : IClientCredential
    {
        [Key]
        public Guid Uid { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime? LastSynced { get; set; }
    }
}
