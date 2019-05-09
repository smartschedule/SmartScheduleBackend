namespace SmartSchedule.Application.Calendar.Commands.CreateCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Persistence;

    public class CreateCalendarCommand : IRequest
    {
        public CreateCalendarRequest Data { get; set; }

        public CreateCalendarCommand()
        {

        }

        public CreateCalendarCommand(CreateCalendarRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<CreateCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CreateCalendarCommand request, CancellationToken cancellationToken)
            {
                CreateCalendarRequest data = request.Data;

                var vResult = await new CreateCalendarCommandValidator(_context).ValidateAsync(data, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var entityCalendar = new Domain.Entities.Calendar
                {
                    Name = data.Name,
                    ColorHex = data.ColorHex
                };
                _context.Calendars.Add(entityCalendar);

                var entityUserCalendar = new Domain.Entities.UserCalendar
                {
                    CalendarId = entityCalendar.Id,
                    UserId = data.UserId
                };
                _context.UserCalendars.Add(entityUserCalendar);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
