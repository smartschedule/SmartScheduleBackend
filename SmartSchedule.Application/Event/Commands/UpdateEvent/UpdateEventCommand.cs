namespace SmartSchedule.Application.Event.Commands.UpdateEvent
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces.UoW;
    using ValidationException = FluentValidation.ValidationException;

    public class UpdateEventCommand : IRequest
    {
        public UpdateEventRequest Data { get; set; }

        public UpdateEventCommand()
        {

        }

        public UpdateEventCommand(UpdateEventRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<UpdateEventCommand, Unit>
        {
            private readonly IUnitOfWork _context;

            public Handler(IUnitOfWork context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
            {
                UpdateEventRequest data = request.Data;

                var entityEvent = await _context.Events.FindAsync(data.Id);

                if (entityEvent == null)
                {
                    throw new NotFoundException("Event", data.Id);
                }

                var vResult = await new UpdateEventCommandValidator(_context).ValidateAsync(data, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var entityLocation = new Domain.Entities.Location
                {
                    Latitude = data.Latitude,
                    Longitude = data.Longitude
                };
                var location = _context.Locations.Add(entityLocation);

                await _context.SaveChangesAsync(cancellationToken);

                entityEvent.StartDate = data.StartDate;
                entityEvent.Duration = data.Duration;
                entityEvent.ReminderBefore = data.ReminderBefore;
                entityEvent.RepeatsEvery = data.RepeatsEvery;
                entityEvent.RepeatsTo = data.RepeatsTo;
                entityEvent.Type = data.Type;
                entityEvent.Name = data.Name;
                entityEvent.ColorHex = data.ColorHex;
                entityEvent.CalendarId = data.CalendarId;
                entityEvent.LocationId = location.Entity.Id;

                _context.Events.Update(entityEvent);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
