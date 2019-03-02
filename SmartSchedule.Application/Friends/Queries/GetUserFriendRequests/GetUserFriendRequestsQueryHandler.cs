namespace SmartSchedule.Application.Friends.Queries.GetUserFriendRequests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.Friends.Models;
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
            throw new System.NotImplementedException();
        }
    }
}
