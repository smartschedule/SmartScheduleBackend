namespace SmartSchedule.Application.Friends.Queries.GetPendingUserFriendRequests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Friends.Queries;
    using SmartSchedule.Application.DTO.User;
    using SmartSchedule.Persistence;

    public class GetPendingUserFriendRequestsQuery : IdRequest, IRequest<FriendsListResponse>
    {
        public class Handler : IRequestHandler<GetPendingUserFriendRequestsQuery, FriendsListResponse>
        {
            private readonly SmartScheduleDbContext _context;
            private readonly IMapper _mapper;

            public Handler(SmartScheduleDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<FriendsListResponse> Handle(GetPendingUserFriendRequestsQuery request, CancellationToken cancellationToken)
            {
                var pendingList = await _context.Friends.Where(x => (x.FirstUserId.Equals(request.Id)
                                                             && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_secound_first))
                                                             || (x.SecoundUserId.Equals(request.Id)
                                                             && (x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_secound))))
                                                             .Include(x => x.FirstUser)
                                                             .Include(x => x.SecoundUser)
                                                             .ToListAsync(cancellationToken);
                var friendsViewModel = new FriendsListResponse
                {
                    Users = new List<UserLookupModel>()
                };

                foreach (var item in pendingList)
                {
                    var user = item.Type == Domain.Enums.FriendshipTypes.pending_secound_first ?
                        item.SecoundUser : item.FirstUser;

                    friendsViewModel.Users.Add(_mapper.Map<UserLookupModel>(user));
                }

                return friendsViewModel;
            }
        }
    }
}
