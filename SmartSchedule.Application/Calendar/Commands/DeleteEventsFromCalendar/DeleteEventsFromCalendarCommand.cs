namespace SmartSchedule.Application.Calendar.Commands.DeleteEventsFromCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;

    public class DeleteEventsFromCalendarCommand : IRequest
    {
        public IdRequest Data { get; set; }

        public DeleteEventsFromCalendarCommand(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteEventsFromCalendarCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteEventsFromCalendarCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var calendar = await _uow.CalendarsRepository.GetByIdAsync(data.Id);
                if (calendar == null)
                {
                    throw new NotFoundException("Calendar", request);
                }

                calendar.Events.Clear();

                _uow.CalendarsRepository.Update(calendar);
                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
