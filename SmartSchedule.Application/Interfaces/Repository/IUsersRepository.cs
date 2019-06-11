namespace SmartSchedule.Application.Interfaces.Repository
{
    using SmartSchedule.Application.Interfaces.Repository.Generic;
    using SmartSchedule.Domain.Entities;

    public interface IUsersRepository : IGenericRepository<User, int>
    {
    }
}
