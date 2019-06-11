namespace SmartSchedule.Infrastructure.Repository
{
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Application.Interfaces.Repository;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class UsersRepository : GenericRepository<User, int>, IUsersRepository
    {
        public UsersRepository(ISmartScheduleDbContext context) : base(context)
        {

        }
    }
}
