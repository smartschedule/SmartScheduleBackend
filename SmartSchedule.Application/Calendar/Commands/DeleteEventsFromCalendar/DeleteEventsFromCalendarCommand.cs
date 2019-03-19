namespace SmartSchedule.Application.Calendar.Commands.DeleteEventsFromCalendar
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteEventsFromCalendarCommand : IRequest
    {
        public int CalendarId { get; set; }

        public class Handler : IRequestHandler<DeleteEventsFromCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(DeleteEventsFromCalendarCommand request, CancellationToken cancellationToken)
            {
                var calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Id.Equals(request.CalendarId));

                if (calendar == null)
                {
                    throw new NotFoundException("Calendar", request);
                }

                calendar.Events.Clear();

                _context.Calendars.Update(calendar);
                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
