namespace SmartSchedule.Application.Calendar.Commands.DeleteFriendFromCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Exceptions;

    public class DeleteFriendFromCalendarCommand : IRequest
    {
        public DeleteFriendFromCalendarRequest Data { get; set; }

        public DeleteFriendFromCalendarCommand(DeleteFriendFromCalendarRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteFriendFromCalendarCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteFriendFromCalendarCommand request, CancellationToken cancellationToken)
            {
                DeleteFriendFromCalendarRequest data = request.Data;

                var userCalendar = await _uow.UserCalendarsRepository.FirstOrDefaultAsync(x => x.CalendarId.Equals(data.CalendarId)
                                                                                    && x.UserId.Equals(data.UserId));

                if (userCalendar == null)
                {
                    throw new NotFoundException("UserCalendar", request);
                }

                _uow.UserCalendarsRepository.Remove(userCalendar);
                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
