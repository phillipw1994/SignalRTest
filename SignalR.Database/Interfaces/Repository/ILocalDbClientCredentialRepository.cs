using System.Threading;
using System.Threading.Tasks;
using SignalR.Database.Interfaces.Repository.Base;
using SignalR.Model.Interfaces;

namespace SignalR.Database.Interfaces.Repository
{
    public interface ILocalDbClientCredentialRepository<T> : IBaseLocalDbRepository<T> where T : class, IClientCredential, ILocalDbEntity
    {
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
    }
}
