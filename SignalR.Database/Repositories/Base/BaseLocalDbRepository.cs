#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalR.Database.Helpers;
using SignalR.Model.Interfaces;

#endregion

namespace SignalR.Database.Repositories.Base
{
    public abstract class BaseLocalDbRepository<T> where T: class, ILocalDbEntity
    {
        protected string ConnectionString { get; }

        #region Constructors

        protected BaseLocalDbRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion

        #region Public Async Methods

        public async Task<List<T>> GetListAsync()
        {
            await using ISignalRDbContext context = new SignalRDbContext(DatabaseHelpers.CreateOptionsBuilder<SignalRDbContext>(ConnectionString).Options);
            var entities = await context.Set<T>().ToListAsync();
            return entities;
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await using ISignalRDbContext context = new SignalRDbContext(DatabaseHelpers.CreateOptionsBuilder<SignalRDbContext>(ConnectionString).Options);
            await context.Set<T>().AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await using ISignalRDbContext context = new SignalRDbContext(DatabaseHelpers.CreateOptionsBuilder<SignalRDbContext>(ConnectionString).Options);
            await context.Set<T>().AddRangeAsync(entities, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            await using ISignalRDbContext context = new SignalRDbContext(DatabaseHelpers.CreateOptionsBuilder<SignalRDbContext>(ConnectionString).Options);
            entity.Active = false;
            entity.Deleted = true;
            entity.LastSynced = null;
            entity.Updated = DateTime.UtcNow;
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid? uid, CancellationToken cancellationToken)
        {
            await using ISignalRDbContext context = new SignalRDbContext(DatabaseHelpers.CreateOptionsBuilder<SignalRDbContext>(ConnectionString).Options);
            var entity = context.Set<T>().FirstOrDefault(t => t.Uid == uid);
            if (entity == null)
                throw new Exception("-Could not remove the record from the database");
            entity.Active = false;
            entity.Deleted = true;
            entity.LastSynced = null;
            entity.Updated = DateTime.UtcNow;
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}