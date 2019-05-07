namespace SmartSchedule.Application.Calendar.Commands.DeleteEventsFromCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class DeleteEventsFromCalendarCommand : IRequest
    {
        public IdRequest Data { get; set; }

        public DeleteEventsFromCalendarCommand()
        {

        }

        public DeleteEventsFromCalendarCommand(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteEventsFromCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteEventsFromCalendarCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

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
