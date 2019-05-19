namespace SmartSchedule.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DAL.Interfaces.Repository;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class FriendsRepository : GenericRepository<Friends, int>, IFriendsRepository
    {
        public FriendsRepository(DbContext context) : base(context)
        {

        }
    }
}
