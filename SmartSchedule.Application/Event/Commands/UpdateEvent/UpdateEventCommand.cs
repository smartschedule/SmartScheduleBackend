namespace SmartSchedule.Application.Event.Commands.UpdateEvent
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Event.Models;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Domain.Enums;
    using SmartSchedule.Persistence;
    using ValidationException = FluentValidation.ValidationException;

    public class UpdateEventCommand : IRequest
    {
        public EventDetailModel EventDetailModel {get;set;}

        public class Handler : IRequestHandler<UpdateEventCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
            {
                var entityEvent = await _context.Events.FindAsync(request.EventDetailModel.Id);

                if (entityEvent == null)
                {
                    throw new NotFoundException("Event", request.EventDetailModel.Id);
                }

                var vResult = await new UpdateEventCommandValidator(_context).ValidateAsync(request.EventDetailModel, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var entityLocation = new Domain.Entities.Location
                {
                    Latitude = request.EventDetailModel.Latitude,
                    Longitude = request.EventDetailModel.Longitude
                };
                var location = _context.Locations.Add(entityLocation);

                await _context.SaveChangesAsync(cancellationToken);

                entityEvent.StartDate = request.EventDetailModel.StartDate;
                entityEvent.Duration = request.EventDetailModel.Duration;
                entityEvent.ReminderBefore = request.EventDetailModel.ReminderBefore;
                entityEvent.RepeatsEvery = request.EventDetailModel.RepeatsEvery;
                entityEvent.RepeatsTo = request.EventDetailModel.RepeatsTo;
                entityEvent.Type = request.EventDetailModel.Type;
                entityEvent.Name = request.EventDetailModel.Name;
                entityEvent.CalendarId = request.EventDetailModel.CalendarId;
                entityEvent.LocationId = location.Entity.Id;


                _context.Events.Update(entityEvent);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
