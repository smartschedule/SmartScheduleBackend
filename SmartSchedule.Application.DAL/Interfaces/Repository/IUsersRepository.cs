namespace SmartSchedule.Application.DAL.Interfaces.Repository
{
    using SmartSchedule.Application.DAL.Interfaces.Repository.Generic;
    using SmartSchedule.Domain.Entities;

    public interface IUsersRepository : IGenericRepository<User, int>
    {
    }
}
