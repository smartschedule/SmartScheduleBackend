namespace SmartSchedule.Infrastructure.Repository.Generic
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DAL.Interfaces.Repository.Generic;
    using SmartSchedule.Domain.Entities.Base;

    public class GenericRepository<TEntity, TId> : GenericReadOnlyRepository<TEntity, TId>, IGenericRepository<TEntity, TId>
        where TEntity : class, IBaseEntity<TId> where TId : IComparable
    {
        public GenericRepository(DbContext context) : base(context)
        {

        }

        public virtual TEntity Add(TEntity entity)
        {
            DateTime time = DateTime.UtcNow;
            entity.Created = time;
            entity.Modified = time;

            var createdEntity = _dbSet.Add(entity);

            return createdEntity.Entity;
        }

        public virtual void Update(TEntity entity)
        {
            entity.Modified = DateTime.UtcNow;

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove(TId id)
        {
            TEntity entity = _dbSet.Find(id);
            Remove(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public virtual Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
