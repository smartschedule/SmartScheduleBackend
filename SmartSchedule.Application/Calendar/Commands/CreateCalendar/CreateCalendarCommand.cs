namespace SmartSchedule.Application.Calendar.Commands.CreateCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Calendar.Commands;

    public class CreateCalendarCommand : IRequest
    {
        public CreateCalendarRequest Data { get; set; }

        public CreateCalendarCommand()
        {

        }

        public CreateCalendarCommand(CreateCalendarRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<CreateCalendarCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(CreateCalendarCommand request, CancellationToken cancellationToken)
            {
                CreateCalendarRequest data = request.Data;

                var vResult = await new CreateCalendarCommandValidator(_uow).ValidateAsync(data, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var entityCalendar = new Domain.Entities.Calendar
                {
                    Name = data.Name,
                    ColorHex = data.ColorHex
                };
                _uow.CalendarsRepository.Add(entityCalendar);

                var entityUserCalendar = new Domain.Entities.UserCalendar
                {
                    CalendarId = entityCalendar.Id,
                    UserId = data.UserId
                };
                _uow.UserCalendarsRepository.Add(entityUserCalendar);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
