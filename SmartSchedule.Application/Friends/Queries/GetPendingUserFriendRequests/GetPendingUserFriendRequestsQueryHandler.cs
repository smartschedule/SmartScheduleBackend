namespace SmartSchedule.Application.Friends.Queries.GetPendingUserFriendRequests
{
    using System;
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

    public class GetPendingUserFriendRequestsQueryHandler : IRequestHandler<GetPendingUserFriendRequestsQuery, FriendsListViewModel>
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetPendingUserFriendRequestsQueryHandler(SmartScheduleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FriendsListViewModel> Handle(GetPendingUserFriendRequestsQuery request, CancellationToken cancellationToken)
        {
            var pendingList = await _context.Friends.Where(x => (x.FirstUserId.Equals(request.UserId)
                                                         && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_secound_first))
                                                         || (x.SecoundUserId.Equals(request.UserId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_secound))))
                                                         .Include(x => x.FirstUser)
                                                         .Include(x => x.SecoundUser)
                                                         .ToListAsync(cancellationToken);
            var friendsViewModel = new FriendsListViewModel
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
