namespace SmartSchedule.Application.Event.Commands.CreateEvent
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Application.Event.Models;
    using SmartSchedule.Persistence;

    public class CreateEventCommand : IRequest
    {
        public EventCreateModel EventCreateModel { get; set; }

        public class Handler : IRequestHandler<CreateEventCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CreateEventCommand request, CancellationToken cancellationToken)
            {

                var vResult = await new CreateEventCommandValidator(_context).ValidateAsync(request.EventCreateModel, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var entityLocation = new Domain.Entities.Location
                {
                    Latitude = request.EventCreateModel.Latitude,
                    Longitude = request.EventCreateModel.Longitude
                };

                var location = _context.Locations.Add(entityLocation);

                await _context.SaveChangesAsync(cancellationToken);

                var entityEvent = new Domain.Entities.Event
                {
                    StartDate = request.EventCreateModel.StartDate,
                    Duration = request.EventCreateModel.Duration,
                    ReminderBefore = request.EventCreateModel.ReminderBefore,
                    RepeatsEvery = request.EventCreateModel.RepeatsEvery,
                    RepeatsTo = request.EventCreateModel.RepeatsTo,
                    Type = request.EventCreateModel.Type,
                    Name = request.EventCreateModel.Name,
                    CalendarId = request.EventCreateModel.CalendarId,
                    LocationId = location.Entity.Id
                };
                _context.Events.Add(entityEvent);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
