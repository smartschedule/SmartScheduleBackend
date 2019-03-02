namespace SmartSchedule.Application.Friends.Queries.GetPendingUserFriendRequests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.Friends.Models;
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
            throw new NotImplementedException();
        }
    }
}
