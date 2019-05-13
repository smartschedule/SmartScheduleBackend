namespace SmartSchedule.Persistence.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Interfaces.Repository;
    using SmartSchedule.Domain.Entities.Base;
    using System;
    using System.Threading.Tasks;

    public class GenericRepository<TId, TContext> : GenericReadOnlyRepository<TId, TContext>, IGenericRepository<TId>
        where TContext : DbContext
    {
        public GenericRepository(TContext context) : base(context)
        {

        }

        public virtual void Create<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity<TId>
        {
            entity.Created = DateTime.UtcNow;
            context.Set<TEntity>().Add(entity);
        }

        public virtual void Update<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity<TId>
        {
            entity.Modified = DateTime.UtcNow;
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete<TEntity>(TId id)
            where TEntity : class, IBaseEntity<TId>
        {
            TEntity entity = context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity<TId>
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
