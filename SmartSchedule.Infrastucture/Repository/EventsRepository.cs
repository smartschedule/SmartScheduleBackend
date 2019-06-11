namespace SmartSchedule.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Interfaces.Repository;
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class EventsRepository : GenericRepository<Event, int>, IEventsRepository
    {
        public EventsRepository(ISmartScheduleDbContext context) : base(context)
        {

        }
    }
}
