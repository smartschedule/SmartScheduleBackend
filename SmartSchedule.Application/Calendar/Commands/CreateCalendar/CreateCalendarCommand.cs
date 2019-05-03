namespace SmartSchedule.Application.Calendar.Commands.CreateCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Persistence;

    public class CreateCalendarCommand : CreateCalendarRequest, IRequest
    {
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
