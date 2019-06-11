namespace SmartSchedule.Application.Event.Queries.GetEvents
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Event.Queries;
    using static SmartSchedule.Application.DTO.Event.Queries.GetEventListResponse;

    public class GetEventsQuery : IRequest<GetEventListResponse>
    {
        public class Handler : IRequestHandler<GetEventsQuery, GetEventListResponse>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetEventListResponse> Handle(GetEventsQuery request, CancellationToken cancellationToken)
            {
                return new GetEventListResponse
                {
                    Events = await _uow.EventsRepository.ProjectTo<EventDetails>(_mapper, cancellationToken)
                };
            }
        }
    }
}
