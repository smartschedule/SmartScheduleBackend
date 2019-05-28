namespace SmartSchedule.Application.Event.Queries.GetEventList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Event.Queries;
    using static SmartSchedule.Application.DTO.Event.Queries.GetEventListResponse;

    public class GetEventListQuery : IRequest<GetEventListResponse>
    {
        public class Handler : IRequestHandler<GetEventListQuery, GetEventListResponse>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetEventListResponse> Handle(GetEventListQuery request, CancellationToken cancellationToken)
            {
                return new GetEventListResponse
                {
                    Events = await _uow.EventsRepository.ProjectTo<EventDetails>(_mapper, cancellationToken)
                };
            }
        }
    }
}
