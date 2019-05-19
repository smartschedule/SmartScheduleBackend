namespace SmartSchedule.Application.DAL.Interfaces.UoW
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using SmartSchedule.Application.DAL.Interfaces.Repository;
    using SmartSchedule.Application.DAL.Interfaces.Repository.Generic;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Domain.Entities.Base;

    public interface IUnitOfWork : IDisposable
    {
        ICalendarsRepository CalendarsRepository { get; }
        IEventsRepository EventsRepository { get; }
        IFriendsRepository FriendsRepository { get; }
        ILocationsRepository LocationsRepository { get; }
        IUsersRepository UsersRepository { get; }
        IUserCalendarsRepository UserCalendarsRepository { get; }
        IUserEventsRepository UserEventsRepository { get; }

        void Dispose(bool disposing);

        IGenericRepository<TEntity, int> Repository<TEntity>() where TEntity : class, IBaseEntity<int>;

        IGenericRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : class, IBaseEntity<TId> where TId : IComparable;

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

