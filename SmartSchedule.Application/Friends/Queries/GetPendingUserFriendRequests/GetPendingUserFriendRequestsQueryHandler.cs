namespace SmartSchedule.Application.Friends.Queries.GetPendingUserFriendRequests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Friends.Models;
    using SmartSchedule.Persistence;

    public class GetPendingUserFriendRequestsQueryHandler : IRequestHandler<GetPendingUserFriendRequestsQuery, FriendsListViewModel>
    {
        private readonly SmartScheduleDbContext _context;

        public GetPendingUserFriendRequestsQueryHandler(SmartScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<FriendsListViewModel> Handle(GetPendingUserFriendRequestsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
