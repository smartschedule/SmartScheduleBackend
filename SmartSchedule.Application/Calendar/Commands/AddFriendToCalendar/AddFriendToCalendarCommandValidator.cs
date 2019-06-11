namespace SmartSchedule.Application.Calendar.Commands.AddFriendToCalendar
{
    using FluentValidation;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Calendar.Commands;

    public class AddFriendToCalendarCommandValidator : AbstractValidator<AddFriendToCalendarRequest>
    {
        public AddFriendToCalendarCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("You must set UserId.");
            RuleFor(x => x.UserId).MustAsync(async (request, val, token) =>
            {
                var userResult = await uow.UsersRepository.GetByIdAsync(val);
                if (userResult == null)
                {
                    return false;
                }

                return true;
            }).WithMessage("This user does not exist.");

            RuleFor(x => x.CalendarId).NotEmpty().WithMessage("You must set CalendarId.");
            RuleFor(x => x.CalendarId).MustAsync(async (request, val, token) =>
            {
                var calendarResult = await uow.CalendarsRepository.GetByIdAsync(val);

                if (calendarResult == null)
                {
                    return false;
                }

                return true;
            }).WithMessage("This calendar does not exist.");

        }
    }
}
