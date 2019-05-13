namespace SmartSchedule.Application.Interfaces.Repository
{
    using SmartSchedule.Domain.Entities.Base;
    using System.Threading.Tasks;

    public interface IGenericRepository<TId> : IReadOnlyRepository<TId>
    {
        void Create<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity<TId>;

        void Update<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity<TId>;

        void Delete<TEntity>(TId id)
            where TEntity : class, IBaseEntity<TId>;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity<TId>;

        void Save();

        Task SaveAsync();
    }
}
