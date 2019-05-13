
namespace SmartSchedule.Application.Interfaces.UoW
{
    using SmartSchedule.Application.Interfaces.Repository;
    using SmartSchedule.Domain.Entities.Base;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        void Dispose(bool disposing);

        IGenericRepository<TEntity, int> Repository<TEntity>() where TEntity : class, IBaseEntity<int>;

        IGenericRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : class, IBaseEntity<TId> where TId : IComparable;

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
