namespace SmartSchedule.Application.Calendar.Queries.GetCalendarDetails
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class GetCalendarDetailQueryHandler : IRequestHandler<GetCalendarDetailQuery, CalendarDetailModel>
    {
        private readonly SmartScheduleDbContext _context;

        public GetCalendarDetailQueryHandler(SmartScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<CalendarDetailModel> Handle(GetCalendarDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Calendars.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Calendar), request.Id);
            }

            var result = CalendarDetailModel.Create(entity);

            result.Events = await _context.Events.Where(x => x.CalendarId == entity.Id).
                                    Select(y => new EventLookupModel
                                    {
                                        Id = y.Id,
                                        Name = y.Name,
                                        CalendarId = entity.Id,
                                        EndTime = y.EndTime,
                                        StartDate = y.StartDate,
                                        Latitude = y.Location.Latitude,
                                        Longitude = y.Location.Longitude

                                    }).ToListAsync();

            return result;
        }
    }
}
