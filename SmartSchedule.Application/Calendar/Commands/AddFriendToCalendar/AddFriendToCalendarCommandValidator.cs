namespace SmartSchedule.Application.Calendar.Commands.AddFriendToCalendar
{
    using FluentValidation;
    using SmartSchedule.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class AddFriendToCalendarCommandValidator : AbstractValidator<AddFriendToCalendarCommand>
    {
        public AddFriendToCalendarCommandValidator(SmartScheduleDbContext context)
        {
            RuleFor(x => x.UserId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                var userResult = await context.Users.FirstOrDefaultAsync(x => x.Id.Equals(val));

                if (userResult == null)
                {
                    return false;
                }

                return true;
            }).WithMessage("This user does not exist.");

            RuleFor(x => x.CalendarId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                var calendarResult = await context.Calendars.FirstOrDefaultAsync(x => x.Id.Equals(val));

                if (calendarResult == null)
                {
                    return false;
                }

                return true;
            }).WithMessage("This calendar does not exist.");
            
        }
    }
}
