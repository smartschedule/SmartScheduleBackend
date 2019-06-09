namespace SmartSchedule.Infrastructure.UoW
{
    using System;
    using System.Collections;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DAL.Interfaces.Repository;
    using SmartSchedule.Application.DAL.Interfaces.Repository.Generic;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Domain.Entities.Base;
    using SmartSchedule.Infrastructure.Repository;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class UnitOfWork : IUnitOfWork
    {
        public ICalendarsRepository CalendarsRepository
        {
            get => Repository<CalendarsRepository, Calendar, int>();
        }
        public IEventsRepository EventsRepository
        {
            get => Repository<EventsRepository, Event, int>();
        }
        public IFriendsRepository FriendsRepository
        {
            get => Repository<FriendsRepository, Friends, int>();
        }
        public ILocationsRepository LocationsRepository
        {
            get => Repository<LocationsRepository, Location, int>();
        }
        public IUsersRepository UsersRepository
        {
            get => Repository<UsersRepository, User, int>();
        }
        public IUserCalendarsRepository UserCalendarsRepository
        {
            get => Repository<UserCalendarsRepository, UserCalendar, int>();
        }
        public IUserEventsRepository UserEventsRepository
        {
            get => Repository<UserEventsRepository, UserEvent, int>();
        }

        private readonly DbContext _context;

        private bool _disposed;

        private Hashtable _repositories;

        public UnitOfWork(DbContext context) //IDbContext do napisania w Application i implementacja w Persistance
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

        public IGenericRepository<TEntity, int> Repository<TEntity>()
            where TEntity : class, IBaseEntity<int>
        {
            return Repository<TEntity, int>();
        }

        public IGenericRepository<TEntity, TId> Repository<TEntity, TId>()
            where TEntity : class, IBaseEntity<TId>
            where TId : IComparable
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

        public TSpecificRepository Repository<TSpecificRepository, TEntity, TId>()
            where TSpecificRepository : IGenericReadOnlyRepository<TEntity, TId>
            where TEntity : class, IBaseEntity<TId>
            where TId : IComparable
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
            {
                return (TSpecificRepository)_repositories[type];
            }

            _repositories.Add(type, Activator.CreateInstance(typeof(TSpecificRepository), _context));
            return (TSpecificRepository)_repositories[type];
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