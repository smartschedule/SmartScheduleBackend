namespace SmartSchedule.Application.DAL.Interfaces.Repository.Generic
{
    using System;
    using System.Threading.Tasks;
    using SmartSchedule.Domain.Entities.Base;

    public interface IGenericRepository<TEntity, TId> : IGenericReadOnlyRepository<TEntity, TId>
        where TEntity : class, IBaseEntity<TId> where TId : IComparable
    {
        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        Task Remove(TId id);

        void Remove(TEntity entity);

        Task SaveAsync();
    }
}