namespace SmartSchedule.Application.Event.Commands.DeleteEvent
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;

    public class DeleteEventCommand : IRequest
    {
        public IdRequest Data { get; set; }

        public DeleteEventCommand(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteEventCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var eventE = await _uow.EventsRepository.GetByIdAsync(data.Id);
                if (eventE == null)
                {
                    throw new NotFoundException("Event", data.Id);
                }

                _uow.EventsRepository.Remove(eventE);
                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
