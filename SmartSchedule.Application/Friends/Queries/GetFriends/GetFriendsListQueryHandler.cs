namespace SmartSchedule.Application.Friends.Queries.GetFriends
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.Friends.Models;
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
            throw new System.NotImplementedException();
        }
    }
}
