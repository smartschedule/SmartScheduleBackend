namespace SmartSchedule.Infrastructure.UoW
{
    using System;
    using System.Collections;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DAL.Interfaces.Repository.Generic;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Domain.Entities.Base;

    public class UnitOfWork : IUnitOfWork
    {
        public IGenericRepository<Calendar, int> CalendarsRepository
        {
            get => Repository<Calendar, int>();
        }
        public IGenericRepository<Event, int> EventsRepository
        {
            get => Repository<Event, int>();
        }
        public IGenericRepository<Friends, int> FriendsRepository
        {
            get => Repository<Friends, int>();
        }
        public IGenericRepository<Location, int> LocationsRepository
        {
            get => Repository<Location, int>();
        }
        public IGenericRepository<User, int> UsersRepository
        {
            get => Repository<User, int>();
        }
        public IGenericRepository<UserCalendar, int> UserCalendarsRepository
        {
            get => Repository<UserCalendar, int>();
        }
        public IGenericRepository<UserEvents, int> UserEventsRepository
        {
            get => Repository<UserEvents, int>();
        }

        private readonly DbContext _context;

        private bool _disposed;

        private Hashtable _repositories;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                if (_repositories != null)
                {
                    foreach (IDisposable repository in _repositories.Values)
                    {
                        repository.Dispose();
                    }
                }
            }

            _disposed = true;
        }

        public IGenericRepository<TEntity, int> Repository<TEntity>() where TEntity : class, IBaseEntity<int>
        {
            return Repository<TEntity, int>();
        }

        public IGenericRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : class, IBaseEntity<TId> where TId : IComparable
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
            {
                return (IGenericRepository<TEntity, TId>)_repositories[type];
            }

            _repositories.Add(type, Activator.CreateInstance(typeof(GenericRepository<TEntity, TId>), _context));
            return (IGenericRepository<TEntity, TId>)_repositories[type];
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}