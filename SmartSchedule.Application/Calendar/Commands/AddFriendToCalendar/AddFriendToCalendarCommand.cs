namespace SmartSchedule.Application.Calendar.Commands.AddFriendToCalendar
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class AddFriendToCalendarCommand : IRequest
    {
        public AddFriendToCalendarRequest Data { get; set; }

        public AddFriendToCalendarCommand()
        {

        }

        public AddFriendToCalendarCommand(AddFriendToCalendarRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<AddFriendToCalendarCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(AddFriendToCalendarCommand request, CancellationToken cancellationToken)
            {
                AddFriendToCalendarRequest data = request.Data;

                var userCalendar = await _uow.UserCalendars.FirstOrDefaultAsync(x => x.CalendarId.Equals(data.CalendarId)
                                                                                       && x.UserId.Equals(data.UserId));
                if (userCalendar != null)
                {
                    throw new ValidationException("This user is already added to this calendar");
                }

                var vResult = await new AddFriendToCalendarCommandValidator(_uow).ValidateAsync(data, cancellationToken);

                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var entityUserCalendar = new Domain.Entities.UserCalendar
                {
                    CalendarId = data.CalendarId,
                    UserId = data.UserId
                };
                _uow.UserCalendars.Add(entityUserCalendar);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
