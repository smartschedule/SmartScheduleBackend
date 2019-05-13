namespace SmartSchedule.Application.Calendar.Commands.UpdateCalendar
{
    using MediatR;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Persistence;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using ValidationException = FluentValidation.ValidationException;

    public class UpdateCalendarCommand : IRequest
    {
        public UpdateCalendarRequest Data { get; set; }

        public UpdateCalendarCommand()
        {

        }

        public UpdateCalendarCommand(UpdateCalendarRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<UpdateCalendarCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateCalendarCommand request, CancellationToken cancellationToken)
            {
                UpdateCalendarRequest data = request.Data;

                var vResult = await new UpdateCalendarCommandValidator(_context).ValidateAsync(data, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }
                var calendar = await _context.Calendars.FindAsync(data.Id);
                calendar.Modified = DateTime.UtcNow;
                calendar.Name = data.Name;
                calendar.ColorHex = data.ColorHex;

                _context.Calendars.Update(calendar);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}