namespace SmartSchedule.Infrastructure.Repository
{
    using SmartSchedule.Application.DAL.Interfaces.Repository;
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class LocationsRepository : GenericRepository<Location, int>, ILocationsRepository
    {
        public LocationsRepository(ISmartScheduleDbContext context) : base(context)
        {

        }
    }
}
