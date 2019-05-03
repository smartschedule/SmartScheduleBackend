namespace SmartSchedule.Application.Event.Commands.UpdateEvent
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using ValidationException = FluentValidation.ValidationException;

    public class UpdateEventCommand : UpdateEventRequest, IRequest
    {
        public class Handler : IRequestHandler<UpdateEventCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
            {
                var entityEvent = await _context.Events.FindAsync(request.Id);

                if (entityEvent == null)
                {
                    throw new NotFoundException("Event", request.Id);
                }

                var vResult = await new UpdateEventCommandValidator(_context).ValidateAsync(request, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var entityLocation = new Domain.Entities.Location
                {
                    Latitude = request.Latitude,
                    Longitude = request.Longitude
                };
                var location = _context.Locations.Add(entityLocation);

                await _context.SaveChangesAsync(cancellationToken);

                entityEvent.StartDate = request.StartDate;
                entityEvent.Duration = request.Duration;
                entityEvent.ReminderBefore = request.ReminderBefore;
                entityEvent.RepeatsEvery = request.RepeatsEvery;
                entityEvent.RepeatsTo = request.RepeatsTo;
                entityEvent.Type = request.Type;
                entityEvent.Name = request.Name;
                entityEvent.CalendarId = request.CalendarId;
                entityEvent.LocationId = location.Entity.Id;

                _context.Events.Update(entityEvent);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
