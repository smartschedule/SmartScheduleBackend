namespace SmartSchedule.Application.Friends.Queries.GetUserFriendRequests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Friends;
    using SmartSchedule.Application.DTO.User;
    using SmartSchedule.Persistence;

    public class GetUserFriendRequestsQueryHandler : IRequestHandler<GetUserFriendRequestsQuery, FriendsListViewModel>
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetUserFriendRequestsQueryHandler(SmartScheduleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<FriendsListViewModel> Handle(GetUserFriendRequestsQuery request, CancellationToken cancellationToken)
        {
            var friendRequestList = await _context.Friends.Where(x => (x.FirstUserId.Equals(request.UserId)
                                                         && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_secound))
                                                         || (x.SecoundUserId.Equals(request.UserId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.pending_secound_first))))
                                                         .Include(x => x.FirstUser)
                                                         .Include(x => x.SecoundUser)
                                                         .ToListAsync(cancellationToken);
            var friendsViewModel = new FriendsListViewModel
            {
                Users = new List<UserLookupModel>()
            };

            foreach (var item in friendRequestList)
            {
                var user = item.Type == Domain.Enums.FriendshipTypes.pending_first_secound ?
                    item.SecoundUser : item.FirstUser;

                friendsViewModel.Users.Add(_mapper.Map<UserLookupModel>(user));
            }

            return friendsViewModel;
        }
    }
}
