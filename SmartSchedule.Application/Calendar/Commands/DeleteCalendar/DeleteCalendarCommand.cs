namespace SmartSchedule.Application.Calendar.Commands.DeleteCalendar
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteCalendarCommand : IRequest
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<DeleteCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteCalendarCommand request, CancellationToken cancellationToken)
            {
                var calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (calendar == null)
                {
                    throw new NotFoundException("Calendar", request.Id);
                }

                _context.Calendars.Remove(calendar);
                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
