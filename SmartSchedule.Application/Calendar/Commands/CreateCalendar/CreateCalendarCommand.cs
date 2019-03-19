namespace SmartSchedule.Application.Calendar.Commands.CreateCalendar
{
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Persistence;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateCalendarCommand : IRequest
    {
        public string Name { get; set; }
        public string ColorHex { get; set; }
        public int UserId { get; set; }

        public class Handler : IRequestHandler<CreateCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(CreateCalendarCommand request, CancellationToken cancellationToken)
            {

                var vResult = await new CreateCalendarCommandValidator(_context).ValidateAsync(request, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var entityCalendar = new Domain.Entities.Calendar
                {
                    Name = request.Name,
                    ColorHex = request.ColorHex
                };
                _context.Calendars.Add(entityCalendar);

                var entityUserCalendar = new Domain.Entities.UserCalendar
                {
                    CalendarId = entityCalendar.Id,
                    UserId = request.UserId
                };
                _context.UserCalendars.Add(entityUserCalendar);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
