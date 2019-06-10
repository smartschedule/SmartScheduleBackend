namespace SmartSchedule.Infrastructure.Repository
{
    using SmartSchedule.Application.DAL.Interfaces.Repository;
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class CalendarsRepository : GenericRepository<Calendar, int>, ICalendarsRepository
    {
        public CalendarsRepository(ISmartScheduleDbContext context) : base(context)
        {

        }
    }
}
