namespace SmartSchedule.Application.Calendar.Commands.DeleteFriendFromCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces.UoW;

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
            private readonly IUnitOfWork _context;

            public Handler(IUnitOfWork context)
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
