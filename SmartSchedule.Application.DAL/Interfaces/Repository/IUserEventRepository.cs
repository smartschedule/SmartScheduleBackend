﻿namespace SmartSchedule.Application.DAL.Interfaces.Repository
{
    using SmartSchedule.Application.DAL.Interfaces.Repository.Generic;
    using SmartSchedule.Domain.Entities;

    public interface IUserEventsRepository : IGenericRepository<UserEvent, int>
    {
    }
}
