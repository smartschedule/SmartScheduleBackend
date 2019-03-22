namespace SmartSchedule.Application.Event.Commands.CreateEvent
{
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Persistence;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateEventCommand : IRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ReminderAt { get; set; }
        public string Name { get; set; }
        public int RepeatsEvery { get; set; }
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
                    EndTime = request.EndTime,
                    ReminderAt = request.ReminderAt,
                    Name = request.Name,
                    RepeatsEvery = request.RepeatsEvery,
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
