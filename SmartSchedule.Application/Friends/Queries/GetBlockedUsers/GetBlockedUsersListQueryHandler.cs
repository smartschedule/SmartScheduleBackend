namespace SmartSchedule.Application.Friends.Queries.GetBlockedUsers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Friends.Models;
    using SmartSchedule.Persistence;

    public class GetBlockedUsersListQueryHandler : IRequestHandler<GetBlockedUsersListQuery, FriendsListViewModel>
    {
        private readonly SmartScheduleDbContext _context;

        public GetBlockedUsersListQueryHandler(SmartScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<FriendsListViewModel> Handle(GetBlockedUsersListQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
