namespace SmartSchedule.Application.DAL.Interfaces.UoW
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using SmartSchedule.Application.DAL.Interfaces.Repository;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Domain.Entities.Base;

    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Calendar, int> CalendarsRepository { get; }
        IGenericRepository<Event, int> EventsRepository { get; }
        IGenericRepository<Friends, int> FriendssRepository { get; }
        IGenericRepository<Location, int> LocationsRepository { get; }
        IGenericRepository<User, int> UsersRepository { get; }
        IGenericRepository<UserCalendar, int> UserCalendarsRepository { get; }
        IGenericRepository<UserEvents, int> UserEventsRepository { get; }

        void Dispose(bool disposing);

        IGenericRepository<TEntity, int> Repository<TEntity>() where TEntity : class, IBaseEntity<int>;

        IGenericRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : class, IBaseEntity<TId> where TId : IComparable;

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

