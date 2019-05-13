namespace SmartSchedule.Application.Calendar.Commands.DeleteEventsFromCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces.UoW;

    public class DeleteEventsFromCalendarCommand : IRequest
    {
        public IdRequest Data { get; set; }

        public DeleteEventsFromCalendarCommand()
        {

        }

        public DeleteEventsFromCalendarCommand(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteEventsFromCalendarCommand, Unit>
        {
            private readonly IUnitOfWork _context;

            public Handler(IUnitOfWork context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteEventsFromCalendarCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

                if (calendar == null)
                {
                    throw new NotFoundException("Calendar", request);
                }

                calendar.Events.Clear();

                _context.Calendars.Update(calendar);
                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
