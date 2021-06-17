#region Usings

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignalR.Model.Interfaces;

#endregion

namespace SignalR.Database.Interfaces.Repository.Base
{
    public interface IBaseLocalDbRepository<T> where T : class, ILocalDbEntity
    {
        #region Public Async Methods

        Task<List<T>> GetListAsync();
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid? uid, CancellationToken cancellationToken);

        #endregion
    }
}