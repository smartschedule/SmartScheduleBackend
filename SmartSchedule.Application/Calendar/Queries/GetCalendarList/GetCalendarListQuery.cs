namespace SmartSchedule.Application.Calendar.Queries.GetCalendarList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Calendar.Queries;
    using SmartSchedule.Persistence;
    using static SmartSchedule.Application.DTO.Calendar.Queries.GetCalendarListResponse;

    public class GetCalendarsListQuery : IRequest<GetCalendarListResponse>
    {
        public GetCalendarsListQuery()
        {

        }

        public class Handler : IRequestHandler<GetCalendarsListQuery, GetCalendarListResponse>
        {
            private readonly SmartScheduleDbContext _context;
            private readonly IMapper _mapper;

            public Handler(SmartScheduleDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<GetCalendarListResponse> Handle(GetCalendarsListQuery request, CancellationToken cancellationToken)
            {
                return new GetCalendarListResponse
                {
                    Calendars = await _context.Calendars.ProjectTo<CalendarLookupModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken)
                };
            }
        }
    }
}
