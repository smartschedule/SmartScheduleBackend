namespace SmartSchedule.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Interfaces.Repository;
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class UserCalendarsRepository : GenericRepository<UserCalendar, int>, IUserCalendarsRepository
    {
        public UserCalendarsRepository(ISmartScheduleDbContext context) : base(context)
        {

        }
    }
}
