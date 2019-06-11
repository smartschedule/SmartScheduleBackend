namespace SmartSchedule.Application.Event.Queries.GetCalendarEvents
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Event.Queries;
    using static SmartSchedule.Application.DTO.Event.Queries.GetEventListResponse;

    //TODO
    public class GetCalendarEventsQuery : IRequest<GetEventListResponse>
    {
        public IdRequest Data { get; set; }

        public GetCalendarEventsQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetCalendarEventsQuery, GetEventListResponse>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetEventListResponse> Handle(GetCalendarEventsQuery request, CancellationToken cancellationToken)
            {
                return new GetEventListResponse
                {
                    Events = await _uow.EventsRepository.ProjectTo<EventDetails>(_mapper, cancellationToken)
                };
            }
        }
    }
}
