namespace SmartSchedule.Application.Event.Queries.GetEventList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using SmartSchedule.Application.DTO.Event.Queries;
    using SmartSchedule.Application.Interfaces.UoW;
    using static SmartSchedule.Application.DTO.Event.Queries.GetEventListResponse;

    public class GetEventListQuery : IRequest<GetEventListResponse>
    {
        public class Handler : IRequestHandler<GetEventListQuery, GetEventListResponse>
        {
            private readonly IUnitOfWork _context;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<GetEventListResponse> Handle(GetEventListQuery request, CancellationToken cancellationToken)
            {
                return new GetEventListResponse
                {
                    Events = await _context.Events.ProjectTo<EventDetails>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken)
                };
            }
        }
    }
}
