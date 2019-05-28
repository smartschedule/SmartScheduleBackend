namespace SmartSchedule.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DAL.Interfaces.Repository;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class EventsRepository : GenericRepository<Event, int>, IEventsRepository
    {
        public EventsRepository(DbContext context) : base(context)
        {

        }
    }
}
