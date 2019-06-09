namespace SmartSchedule.Application.Event.Queries.GetUserEvents
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Event.Queries;
    using static SmartSchedule.Application.DTO.Event.Queries.GetEventListResponse;

    //TODO
    public class GetUserEventsQuery : IRequest<GetEventListResponse>
    {
        public IdRequest Data { get; set; }

        public GetUserEventsQuery(IdRequest data)
        {
            this.Data = data;
        }


        public class Handler : IRequestHandler<GetUserEventsQuery, GetEventListResponse>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetEventListResponse> Handle(GetUserEventsQuery request, CancellationToken cancellationToken)
            {
                return new GetEventListResponse
                {
                    Events = await _uow.EventsRepository.ProjectTo<EventDetails>(_mapper, cancellationToken)
                };
            }
        }
    }
}
