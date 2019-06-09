namespace SmartSchedule.Application.Calendar.Queries.GetUserCalendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.Calendar.Queries.GetCalendars;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Calendar.Queries;
    using SmartSchedule.Application.DTO.Common;
    using static SmartSchedule.Application.DTO.Calendar.Queries.GetCalendarListResponse;

    //TODO
    public class GetUserCalendarsQuery : IRequest<GetCalendarListResponse>
    {
        public IdRequest Data { get; set; }

        public GetUserCalendarsQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetCalendarsQuery, GetCalendarListResponse>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetCalendarListResponse> Handle(GetCalendarsQuery request, CancellationToken cancellationToken)
            {
                return new GetCalendarListResponse
                {
                    Calendars = await _uow.CalendarsRepository.ProjectTo<CalendarLookupModel>(_mapper, cancellationToken)
                };
            }
        }
    }
}
