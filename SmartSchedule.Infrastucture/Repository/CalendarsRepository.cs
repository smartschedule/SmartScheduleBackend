namespace SmartSchedule.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Interfaces.Repository;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class CalendarsRepository : GenericRepository<Calendar, int>, ICalendarsRepository
    {
        public CalendarsRepository(DbContext context) : base(context)
        {

        }
    }
}
