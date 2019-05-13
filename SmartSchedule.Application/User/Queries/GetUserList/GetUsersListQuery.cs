namespace SmartSchedule.Application.User.Queries.GetUserList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using SmartSchedule.Application.DTO.User;
    using SmartSchedule.Application.DTO.User.Queries;
    using SmartSchedule.Application.Interfaces.UoW;

    public class GetUsersListQuery : IRequest<GetUsersListResponse>
    {
        public class Handler : IRequestHandler<GetUsersListQuery, GetUsersListResponse>
        {
            private readonly IUnitOfWork _context;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<GetUsersListResponse> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
            {
                return new GetUsersListResponse
                {
                    Users = await _context.Users.ProjectTo<UserLookupModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken)
                };
            }
        }
    }
}
