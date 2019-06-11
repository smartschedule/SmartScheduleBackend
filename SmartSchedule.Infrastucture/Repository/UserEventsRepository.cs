﻿namespace SmartSchedule.Infrastructure.Repository
{
    using SmartSchedule.Application.DAL.Interfaces.Repository;
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class UserEventsRepository : GenericRepository<UserEvent, int>, IUserEventsRepository
    {
        public UserEventsRepository(ISmartScheduleDbContext context) : base(context)
        {

        }
    }
}
