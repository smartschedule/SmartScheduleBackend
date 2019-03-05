namespace SmartSchedule.Application.Friends.Queries.GetBlockedUsers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Friends.Models;
    using SmartSchedule.Application.Models;
    using SmartSchedule.Persistence;

    public class GetBlockedUsersListQueryHandler : IRequestHandler<GetBlockedUsersListQuery, FriendsListViewModel>
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetBlockedUsersListQueryHandler(SmartScheduleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<FriendsListViewModel> Handle(GetBlockedUsersListQuery request, CancellationToken cancellationToken)
        {
            var blockedList = await _context.Friends.Where(x => (x.FirstUserId.Equals(request.UserId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_first_secound)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both)))
                                                         || (x.SecoundUserId.Equals(request.UserId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_scound_first)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both))))
                                                         .Include(x => x.FirstUser)
                                                         .Include(x => x.SecoundUser)
                                                         .ToListAsync(cancellationToken);
            var friendsViewModel = new FriendsListViewModel
            {
                Users = new List<UserLookupModel>()
            };

            foreach (var item in blockedList)
            {
                var user = item.Type == Domain.Enums.FriendshipTypes.block_first_secound ?
                    item.SecoundUser : item.FirstUser;
                if (item.Type.Equals(Domain.Enums.FriendshipTypes.block_both))
                {
                    user = item.FirstUserId.Equals(request.UserId) ? item.SecoundUser : item.FirstUser;
                }
                friendsViewModel.Users.Add(_mapper.Map<UserLookupModel>(user));
            }

            return friendsViewModel;
        }
    }
}
