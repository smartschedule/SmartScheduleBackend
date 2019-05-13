namespace SmartSchedule.Application.Calendar.Commands.DeleteCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces.UoW;

    public class DeleteCalendarCommand : IRequest
    {
        public IdRequest Data { get; set; }

        public DeleteCalendarCommand()
        {

        }

        public DeleteCalendarCommand(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteCalendarCommand, Unit>
        {
            private readonly IUnitOfWork _context;

            public Handler(IUnitOfWork context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteCalendarCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

                if (calendar == null)
                {
                    throw new NotFoundException("Calendar", data.Id);
                }

                _context.Calendars.Remove(calendar);
                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
