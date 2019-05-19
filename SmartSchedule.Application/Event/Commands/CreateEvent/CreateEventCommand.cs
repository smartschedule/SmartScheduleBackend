namespace SmartSchedule.Application.Event.Commands.CreateEvent
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class CreateEventCommand : IRequest
    {
        public CreateEventRequest Data { get; set; }

        public CreateEventCommand()
        {

        }

        public CreateEventCommand(CreateEventRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<CreateEventCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(CreateEventCommand request, CancellationToken cancellationToken)
            {
                CreateEventRequest data = request.Data;

                var vResult = await new CreateEventCommandValidator(_uow).ValidateAsync(data, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var entityLocation = new Domain.Entities.Location
                {
                    Latitude = data.Latitude,
                    Longitude = data.Longitude
                };

                var location = _uow.LocationsRepository.Add(entityLocation);

                await _uow.SaveChangesAsync(cancellationToken);

                var entityEvent = new Domain.Entities.Event
                {
                    StartDate = data.StartDate,
                    Duration = data.Duration,
                    ReminderBefore = data.ReminderBefore,
                    RepeatsEvery = data.RepeatsEvery,
                    RepeatsTo = data.RepeatsTo,
                    Type = data.Type,
                    Name = data.Name,
                    ColorHex = data.ColorHex,
                    CalendarId = data.CalendarId,
                    LocationId = location.Id
                };

                _uow.EventsRepository.Add(entityEvent);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
