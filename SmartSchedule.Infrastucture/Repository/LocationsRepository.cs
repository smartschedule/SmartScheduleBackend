namespace SmartSchedule.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DAL.Interfaces.Repository;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class LocationsRepository : GenericRepository<Location, int>, ILocationsRepository
    {
        public LocationsRepository(DbContext context) : base(context)
        {

        }
    }
}
