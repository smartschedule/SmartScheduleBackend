namespace SmartSchedule.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Interfaces.Repository;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class UserCalendarsRepository : GenericRepository<UserCalendar, int>, IUserCalendarsRepository
    {
        public UserCalendarsRepository(DbContext context) : base(context)
        {

        }
    }
}
