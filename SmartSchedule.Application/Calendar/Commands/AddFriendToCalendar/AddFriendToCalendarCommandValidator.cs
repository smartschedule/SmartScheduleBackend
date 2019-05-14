namespace SmartSchedule.Application.Calendar.Commands.AddFriendToCalendar
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Interfaces.UoW;

    public class AddFriendToCalendarCommandValidator : AbstractValidator<AddFriendToCalendarRequest>
    {
        public AddFriendToCalendarCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("You must set UserId.");
            RuleFor(x => x.UserId).MustAsync(async (request, val, token) =>
            {
                var userResult = await uow.Users.FirstOrDefaultAsync(x => x.Id.Equals(val));

                if (userResult == null)
                {
                    return false;
                }

                return true;
            }).WithMessage("This user does not exist.");

            RuleFor(x => x.CalendarId).NotEmpty().WithMessage("You must set CalendarId.");
            RuleFor(x => x.CalendarId).MustAsync(async (request, val, token) =>
            {
                var calendarResult = await uow.Calendars.FirstOrDefaultAsync(x => x.Id.Equals(val));

                if (calendarResult == null)
                {
                    return false;
                }

                return true;
            }).WithMessage("This calendar does not exist.");

        }
    }
}
