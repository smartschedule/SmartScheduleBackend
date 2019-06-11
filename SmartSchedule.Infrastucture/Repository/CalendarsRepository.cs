namespace SmartSchedule.Infrastructure.Repository
{
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Application.Interfaces.Repository;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class CalendarsRepository : GenericRepository<Calendar, int>, ICalendarsRepository
    {
        public CalendarsRepository(ISmartScheduleDbContext context) : base(context)
        {

        }
    }
}
