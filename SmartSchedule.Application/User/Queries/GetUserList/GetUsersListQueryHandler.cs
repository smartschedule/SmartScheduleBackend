using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSchedule.Persistence;

namespace SmartSchedule.Application.User.Queries.GetUserList
{
    public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, UserListViewModel>
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetUsersListQueryHandler(SmartScheduleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UserListViewModel> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            return new UserListViewModel
            {
                Users = await _context.Users.ProjectTo<UserLookupModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken)
            };
        }
    }
}
