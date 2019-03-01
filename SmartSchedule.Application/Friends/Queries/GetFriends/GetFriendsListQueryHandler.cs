namespace SmartSchedule.Application.Friends.Queries.GetFriends
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Friends.Models;
    using SmartSchedule.Persistence;

    public class GetFriendsListQueryHandler : IRequestHandler<GetFriendsListQuery, FriendsListViewModel>
    {
        private readonly SmartScheduleDbContext _context;

        public GetFriendsListQueryHandler(SmartScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<FriendsListViewModel> Handle(GetFriendsListQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
