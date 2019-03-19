namespace SmartSchedule.Application.Calendar.Queries.GetCalendarList
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Persistence;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetCalendarsListQueryHandler : IRequestHandler<GetCalendarsListQuery, CalendarListViewModel>
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetCalendarsListQueryHandler(SmartScheduleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CalendarListViewModel> Handle(GetCalendarsListQuery request, CancellationToken cancellationToken)
        {
            return new CalendarListViewModel
            {
                Calendars = await _context.Calendars.ProjectTo<CalendarLookupModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken)
            };
        }
    }
}
