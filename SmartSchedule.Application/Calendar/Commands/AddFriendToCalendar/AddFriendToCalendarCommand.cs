namespace SmartSchedule.Application.Calendar.Commands.AddFriendToCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Persistence;

    public class AddFriendToCalendarCommand : IRequest
    {
        public int CalendarId { get; set; }
        public int UserId { get; set; }

        public class Handler : IRequestHandler<AddFriendToCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(AddFriendToCalendarCommand request, CancellationToken cancellationToken)
            {
                var userCalendar = await _context.UserCalendars.FirstOrDefaultAsync(x => x.CalendarId.Equals(request.CalendarId)
                                                                                       && x.UserId.Equals(request.UserId));
                if (userCalendar != null)
                {
                    throw new ValidationException("This user is already added to this calendar");
                }

                var vResult = await new AddFriendToCalendarCommandValidator(_context).ValidateAsync(request, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var entityUserCalendar = new Domain.Entities.UserCalendar
                {
                    CalendarId = request.CalendarId,
                    UserId = request.UserId
                };
                _context.UserCalendars.Add(entityUserCalendar);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
