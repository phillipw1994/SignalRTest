using System;
using System.Threading;
using System.Threading.Tasks;
using SignalR.Database.Helpers;
using SignalR.Database.Interfaces.Repository;
using SignalR.Database.Repositories.Base;
using SignalR.Model.Interfaces;

namespace SignalR.Database.Repositories
{
    public class LocalDbClientCredentialRepository<T> : BaseLocalDbRepository<T>, ILocalDbClientCredentialRepository<T> where T : class, IClientCredential, ILocalDbEntity,  new()
    {
        public LocalDbClientCredentialRepository(string connectionString)
            : base(connectionString)
        {
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            await using ISignalRDbContext context = new SignalRDbContext(DatabaseHelpers.CreateOptionsBuilder<SignalRDbContext>(ConnectionString).Options);
            entity.Updated = DateTime.UtcNow;
            context.Set<T>().Update(entity);
            //var entityNew = context.Set<T>().Update(entity);
            //entityNew.DetectChanges();
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}