namespace SmartSchedule.Application.Event.Commands.UpdateEvent
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using ValidationException = FluentValidation.ValidationException;

    public class UpdateEventCommand : IRequest
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ReminderAt { get; set; }
        public string Name { get; set; }
        public int RepeatsEvery { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

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
                _context.Locations.Add(entityLocation);

                entityEvent.StartDate = request.StartDate;
                entityEvent.EndTime = request.EndTime;
                entityEvent.ReminderAt = request.ReminderAt;
                entityEvent.Name = request.Name;
                entityEvent.RepeatsEvery = request.RepeatsEvery;
                entityEvent.LocationId = entityLocation.Id;

                _context.Events.Update(entityEvent);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
