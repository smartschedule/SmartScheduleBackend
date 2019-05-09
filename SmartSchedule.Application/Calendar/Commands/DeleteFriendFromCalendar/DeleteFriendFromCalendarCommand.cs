namespace SmartSchedule.Application.Calendar.Commands.DeleteFriendFromCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class DeleteFriendFromCalendarCommand : IRequest
    {
        public DeleteFriendFromCalendarRequest Data { get; set; }

        public DeleteFriendFromCalendarCommand()
        {

        }

        public DeleteFriendFromCalendarCommand(DeleteFriendFromCalendarRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteFriendFromCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteFriendFromCalendarCommand request, CancellationToken cancellationToken)
            {
                DeleteFriendFromCalendarRequest data = request.Data;

                var userCalendar = await _context.UserCalendars.FirstOrDefaultAsync(x => x.CalendarId.Equals(data.CalendarId)
                                                                                    && x.UserId.Equals(data.UserId));

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
