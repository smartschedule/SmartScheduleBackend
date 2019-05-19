namespace SmartSchedule.Application.User.Queries.GetUserList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using SmartSchedule.Application.DTO.User;
    using SmartSchedule.Application.DTO.User.Queries;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class GetUsersListQuery : IRequest<GetUsersListResponse>
    {
        public class Handler : IRequestHandler<GetUsersListQuery, GetUsersListResponse>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetUsersListResponse> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
            {
                return new GetUsersListResponse
                {
                    Users = await _uow.UsersRepository.ProjectTo<UserLookupModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken)
                };
            }
        }
    }
}
