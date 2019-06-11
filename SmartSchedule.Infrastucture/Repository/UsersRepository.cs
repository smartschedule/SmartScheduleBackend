namespace SmartSchedule.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Interfaces.Repository;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class UsersRepository : GenericRepository<User, int>, IUsersRepository
    {
        public UsersRepository(DbContext context) : base(context)
        {

        }
    }
}
