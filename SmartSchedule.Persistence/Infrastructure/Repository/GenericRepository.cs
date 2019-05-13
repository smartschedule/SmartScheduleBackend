namespace SmartSchedule.Persistence.Infrastructure.Repository
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Interfaces.Repository;
    using SmartSchedule.Domain.Entities.Base;

    public class GenericRepository<TEntity, TId> : GenericReadOnlyRepository<TEntity, TId>, IGenericRepository<TEntity, TId>
        where TEntity : class, IBaseEntity<TId> where TId : IComparable
    {
        public GenericRepository(DbContext context) : base(context)
        {

        }

        public virtual void Create(TEntity entity)
        {
            DateTime time = DateTime.UtcNow;
            entity.Created = time;
            entity.Modified = time;

            context.Set<TEntity>().Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            entity.Modified = DateTime.UtcNow;

            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TId id)
        {
            TEntity entity = context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            var dbSet = context.Set<TEntity>();
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            dbSet.Remove(entity);
        }

        public virtual void Save()
        {
            context.SaveChanges();
        }

        public virtual Task SaveAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
