namespace SmartSchedule.Infrastructure.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DAL.Interfaces.Repository;
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Infrastructure.Repository.Generic;

    public class FriendsRepository : GenericRepository<Friends, int>, IFriendsRepository
    {
        public FriendsRepository(ISmartScheduleDbContext context) : base(context)
        {

        }

        public async Task<List<Friends>> GetBlockedFriends(int userId, int friendId, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(x => (x.FirstUserId.Equals(userId) && x.SecoundUserId.Equals(friendId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_first_second)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both)))
                                                         || (x.SecoundUserId.Equals(userId) && x.FirstUserId.Equals(friendId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_second_first)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both))))
                                                         .ToListAsync(cancellationToken);
        }

        public async Task<List<Friends>> GetBlockedUsers(int userId, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(x => (x.FirstUserId.Equals(userId)
                                                             && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_first_second)
                                                             || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both)))
                                                             || (x.SecoundUserId.Equals(userId)
                                                             && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_second_first)
                                                             || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both))))
                                                             .Include(x => x.FirstUser)
                                                             .Include(x => x.SecoundUser)
                                                             .ToListAsync(cancellationToken);
        }

        public async Task<List<Friends>> GetFriends(int userId, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(x => (x.FirstUserId.Equals(userId)
                                                             || x.SecoundUserId.Equals(userId))
                                                             && x.Type.Equals(Domain.Enums.FriendshipTypes.friends))
                                                             .Include(x => x.FirstUser)
                                                             .Include(x => x.SecoundUser)
                                                             .ToListAsync(cancellationToken);
        }

        public async Task<List<Friends>> GetPendingFriends(int userId, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(x => (x.FirstUserId.Equals(userId)
                                                             && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_second_first))
                                                             || (x.SecoundUserId.Equals(userId)
                                                             && (x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_second))))
                                                             .Include(x => x.FirstUser)
                                                             .Include(x => x.SecoundUser)
                                                             .ToListAsync(cancellationToken);
        }

        public async Task<List<Friends>> GetPendingUserFriends(int userId, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(x => (x.FirstUserId.Equals(userId)
                                                            && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_second))
                                                            || (x.SecoundUserId.Equals(userId)
                                                            && (x.Type.Equals(Domain.Enums.FriendshipTypes.pending_second_first))))
                                                            .Include(x => x.FirstUser)
                                                            .Include(x => x.SecoundUser)
                                                            .ToListAsync(cancellationToken);
        }
    }
}
