namespace SmartSchedule.Application.Friends.Queries.GetUserFriendRequests
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Friends.Models;
    using SmartSchedule.Persistence;

    public class GetUserFriendRequestsQueryHandler : IRequestHandler<GetUserFriendRequestsQuery, FriendsListViewModel>
    {
        private readonly SmartScheduleDbContext _context;

        public GetUserFriendRequestsQueryHandler(SmartScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<FriendsListViewModel> Handle(GetUserFriendRequestsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
