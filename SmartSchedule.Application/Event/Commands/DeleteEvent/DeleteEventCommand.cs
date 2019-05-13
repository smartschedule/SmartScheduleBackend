namespace SmartSchedule.Application.Event.Commands.DeleteEvent
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Persistence;

    public class DeleteEventCommand : IRequest
    {
        public IdRequest Data { get; set; }

        public DeleteEventCommand()
        {

        }

        public DeleteEventCommand(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteEventCommand, Unit>
        {
            private readonly IUnitOfWork _context;

            public Handler(IUnitOfWork context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var eventE = await _context.Events.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

                if (eventE == null)
                {
                    throw new NotFoundException("Event", data.Id);
                }

                _context.Events.Remove(eventE);
                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
