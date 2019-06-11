namespace SmartSchedule.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Interfaces.Repository;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class UserEventsRepository : GenericRepository<UserEvent, int>, IUserEventsRepository
    {
        public UserEventsRepository(DbContext context) : base(context)
        {

        }
    }
}
