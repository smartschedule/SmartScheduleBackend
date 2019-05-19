namespace SmartSchedule.Application.DAL.Interfaces.Repository
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using SmartSchedule.Application.DAL.Interfaces.Repository.Generic;
    using SmartSchedule.Domain.Entities;

    public interface IFriendsRepository : IGenericRepository<Friends, int>
    {
        Task<List<Friends>> GetBlockedFriends(int userId, int friendId, CancellationToken cancellationToken);
        Task<List<Friends>> GetBlockedUsers(int userId, CancellationToken cancellationToken);
        Task<List<Friends>> GetFriends(int userId, CancellationToken cancellationToken);
        Task<List<Friends>> GetPendingFriends(int userId, CancellationToken cancellationToken);
    }
}
