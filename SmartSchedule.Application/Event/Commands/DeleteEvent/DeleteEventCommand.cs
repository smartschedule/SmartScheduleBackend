namespace SmartSchedule.Application.Event.Commands.DeleteEvent
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class DeleteEventCommand : IRequest
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<DeleteEventCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
            {
                var eventE = await _context.Events.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (eventE == null)
                {
                    throw new NotFoundException("Event", request.Id);
                }

                _context.Events.Remove(eventE);
                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
