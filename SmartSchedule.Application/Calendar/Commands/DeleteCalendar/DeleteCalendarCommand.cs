namespace SmartSchedule.Application.Calendar.Commands.DeleteCalendar
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteCalendarCommand : DeleteCalendarRequest, IRequest
    {
        public class Handler : IRequestHandler<DeleteCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteCalendarCommand request, CancellationToken cancellationToken)
            {
                var calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Id.Equals(request.CalendarId));

                if (calendar == null)
                {
                    throw new NotFoundException("Calendar", request.CalendarId);
                }

                _context.Calendars.Remove(calendar);
                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
