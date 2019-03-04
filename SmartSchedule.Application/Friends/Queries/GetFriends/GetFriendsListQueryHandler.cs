namespace SmartSchedule.Application.Friends.Queries.GetFriends
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

    public class GetFriendsListQueryHandler : IRequestHandler<GetFriendsListQuery, FriendsListViewModel>
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetFriendsListQueryHandler(SmartScheduleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FriendsListViewModel> Handle(GetFriendsListQuery request, CancellationToken cancellationToken)
        {
            var friendsList = await _context.Friends.Where(x => (x.FirstUserId.Equals(request.UserId)
                                                         || x.SecoundUserId.Equals(request.UserId))
                                                         && x.Type.Equals(Domain.Enums.FriendshipTypes.friends))
                                                         .Include(x => x.FirstUser)
                                                         .Include(x => x.SecoundUser)
                                                         .ToListAsync(cancellationToken);
            var friendsViewModel = new FriendsListViewModel
            {
                Users = new List<UserLookupModel>()
            };

            foreach (var item in friendsList)
            {
                var user = item.FirstUserId.Equals(request.UserId) ? item.SecoundUser : item.FirstUser;
                friendsViewModel.Users.Add(_mapper.Map<UserLookupModel>(user));
            }

            return friendsViewModel;
        }
    }
}
