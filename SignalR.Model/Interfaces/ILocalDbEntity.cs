#region Usings

using System;

#endregion

namespace SignalR.Model.Interfaces
{
    public interface ILocalDbEntity
    {
        #region Public Properties

        Guid Uid { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime? LastSynced { get; set; }

        #endregion
    }
}
