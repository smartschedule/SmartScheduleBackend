namespace SmartSchedule.Application.Calendar.Commands.DeleteCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;

    public class DeleteCalendarCommand : IRequest
    {
        public IdRequest Data { get; set; }

        public DeleteCalendarCommand(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteCalendarCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteCalendarCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var calendar = await _uow.CalendarsRepository.GetByIdAsync(data.Id);
                if (calendar == null)
                {
                    throw new NotFoundException("Calendar", data.Id);
                }

                _uow.CalendarsRepository.Remove(calendar);
                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
