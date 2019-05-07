namespace SmartSchedule.Application.Calendar.Commands.DeleteCalendar
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteCalendarCommand : IRequest
    {
        public DeleteCalendarRequest Data { get; set; }

        public DeleteCalendarCommand()
        {

        }

        public DeleteCalendarCommand(DeleteCalendarRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteCalendarCommand request, CancellationToken cancellationToken)
            {
                DeleteCalendarRequest data = request.Data;

                var calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Id.Equals(data.CalendarId));

                if (calendar == null)
                {
                    throw new NotFoundException("Calendar", data.CalendarId);
                }

                _context.Calendars.Remove(calendar);
                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
