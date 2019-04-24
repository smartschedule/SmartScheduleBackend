namespace SmartSchedule.Application.Calendar.Commands.DeleteFriendFromCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class DeleteFriendFromCalendarCommand : IRequest
    {
        public int CalendarId { get; set; }
        public int UserId { get; set; }

        public class Handler : IRequestHandler<DeleteFriendFromCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteFriendFromCalendarCommand request, CancellationToken cancellationToken)
            {
                var userCalendar = await _context.UserCalendars.FirstOrDefaultAsync(x => x.CalendarId.Equals(request.CalendarId)
                                                                                    && x.UserId.Equals(request.UserId));

                if (userCalendar == null)
                {
                    throw new NotFoundException("UserCalendar", request);
                }

                _context.UserCalendars.Remove(userCalendar);
                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
