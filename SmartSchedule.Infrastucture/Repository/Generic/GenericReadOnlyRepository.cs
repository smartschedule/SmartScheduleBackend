namespace SmartSchedule.Infrastructure.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DAL.Interfaces.Repository.Generic;
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Domain.Entities.Base;

    public class GenericReadOnlyRepository<TEntity, TId> : IGenericReadOnlyRepository<TEntity, TId>, IDisposable
        where TEntity : class, IBaseEntity<TId>
        where TId : IComparable
    {
        protected readonly ISmartScheduleDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericReadOnlyRepository(ISmartScheduleDbContext context)
        {
            this._context = context;
            this._dbSet = ((DbContext)context).Set<TEntity>();
        }

        protected virtual IQueryable<TEntity> GetQueryable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            return await GetQueryable(null, orderBy).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            return await GetQueryable(filter, orderBy).ToListAsync();
        }


        public virtual async Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> filter = null)
        {
            return await GetQueryable(filter, null).SingleOrDefaultAsync();
        }


        public virtual async Task<TEntity> GetFirstAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            return await GetQueryable(filter, orderBy).FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual Task<TEntity> GetByIdAsync(TId id)
        {
            return _dbSet.FindAsync(id);
            //return _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).CountAsync();
        }

        public virtual Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).AnyAsync();
        }

        public virtual async Task<IList<T>> ProjectTo<T>(IMapper mapper, CancellationToken cancellationToken)
        {
            return await _dbSet.ProjectTo<T>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }

        public void Dispose()
        {
            //_context.Dispose();
        }
    }
}
