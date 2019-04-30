namespace SmartSchedule.Application.Event.Commands.CreateEvent
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Domain.Enums;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Persistence;

    public class CreateEventCommand : IRequest
    {
        public DateTime StartDate { get; set; }
        public TimeSpan Duration { get; set; }

        public TimeSpan? ReminderBefore { get; set; }

        public TimeSpan? RepeatsEvery { get; set; }
        public DateTime? RepeatsTo { get; set; }

        public EventTypes Type { get; set; }

        public string Name { get; set; }
        public int CalendarId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public class Handler : IRequestHandler<CreateEventCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CreateEventCommand request, CancellationToken cancellationToken)
            {

                var vResult = await new CreateEventCommandValidator(_context).ValidateAsync(request, cancellationToken);

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

                var entityEvent = new Domain.Entities.Event
                {
                    StartDate = request.StartDate,
                    Duration = request.Duration,
                    ReminderBefore = request.ReminderBefore,
                    RepeatsEvery = request.RepeatsEvery,
                    RepeatsTo = request.RepeatsTo,
                    Type = request.Type,
                    Name = request.Name,
                    CalendarId = request.CalendarId,
                    LocationId = location.Entity.Id
                };
                _context.Events.Add(entityEvent);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
