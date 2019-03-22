namespace SmartSchedule.Application.Calendar.Commands.UpdateCalendar
{
    using MediatR;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using System.Threading;
    using System.Threading.Tasks;
    using ValidationException = FluentValidation.ValidationException;

    public class UpdateCalendarCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorHex { get; set; }

        public class Handler : IRequestHandler<UpdateCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(UpdateCalendarCommand request, CancellationToken cancellationToken)
            {
                var calendar = await _context.Calendars.FindAsync(request.Id);

                if (calendar == null)
                {
                    throw new NotFoundException("Calendar", request.Id);
                }

                var vResult = await new UpdateCalendarCommandValidator(_context).ValidateAsync(request, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                calendar.Name = request.Name;
                calendar.ColorHex = request.ColorHex;

                _context.Calendars.Update(calendar);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
