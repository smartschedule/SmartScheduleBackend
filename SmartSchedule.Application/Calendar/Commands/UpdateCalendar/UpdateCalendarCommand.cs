namespace SmartSchedule.Application.Calendar.Commands.UpdateCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using ValidationException = FluentValidation.ValidationException;

    public class UpdateCalendarCommand : IRequest
    {
        public UpdateCalendarRequest Data { get; set; }

        public UpdateCalendarCommand(UpdateCalendarRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<UpdateCalendarCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(UpdateCalendarCommand request, CancellationToken cancellationToken)
            {
                UpdateCalendarRequest data = request.Data;

                var vResult = await new UpdateCalendarCommandValidator(_uow).ValidateAsync(data, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var calendar = await _uow.CalendarsRepository.GetByIdAsync(data.Id);
                calendar.Name = data.Name;
                calendar.ColorHex = data.ColorHex;

                _uow.CalendarsRepository.Update(calendar);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
