namespace SmartSchedule.Application.DAL.Interfaces.Repository
{
    using System;
    using SmartSchedule.Application.DAL.Interfaces.Repository.Generic;
    using SmartSchedule.Domain.Entities.Base;

    public interface IUserEventsRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
        where TEntity : class, IBaseEntity<TId> where TId : IComparable
    {
    }
}
