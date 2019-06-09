namespace SmartSchedule.Application.Calendar.Queries.GetUserCalendarList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.Calendar.Queries.GetCalendarList;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Calendar.Queries;
    using static SmartSchedule.Application.DTO.Calendar.Queries.GetCalendarListResponse;

    public class GetUserCalendarsListQuery : IRequest<GetCalendarListResponse>
    {
        public class Handler : IRequestHandler<GetCalendarsListQuery, GetCalendarListResponse>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetCalendarListResponse> Handle(GetCalendarsListQuery request, CancellationToken cancellationToken)
            {
                return new GetCalendarListResponse
                {
                    Calendars = await _uow.CalendarsRepository.ProjectTo<CalendarLookupModel>(_mapper, cancellationToken)
                };
            }
        }
    }
}
