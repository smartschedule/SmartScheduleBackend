namespace SmartSchedule.Application.Interfaces.Repository
{
    using SmartSchedule.Domain.Entities.Base;
    using System;
    using System.Threading.Tasks;

    public interface IGenericRepository<TEntity, TId> : IGenericReadOnlyRepository<TEntity, TId>
        where TEntity : class, IBaseEntity<TId> where TId : IComparable
    {
        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TId id);

        void Delete(TEntity entity);

        void Save();

        Task SaveAsync();
    }
}