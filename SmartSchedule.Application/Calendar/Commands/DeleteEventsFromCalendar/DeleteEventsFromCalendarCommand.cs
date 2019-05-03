namespace SmartSchedule.Application.Calendar.Commands.DeleteEventsFromCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class DeleteEventsFromCalendarCommand : DeleteEventsFromCalendarRequest, IRequest
    {
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
