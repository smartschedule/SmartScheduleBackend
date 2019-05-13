namespace SmartSchedule.Application.Calendar.Commands.UpdateCalendar
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Persistence;

    public class UpdateCalendarCommandValidator : AbstractValidator<UpdateCalendarRequest>
    {
        public UpdateCalendarCommandValidator(SmartScheduleDbContext context)
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("You must set Id.");
            RuleFor(x => x.Id).MustAsync(async (request, val, token) =>
            {
                var userResult = await context.Calendars.FindAsync(val);

                if (userResult == null)
                {
                    return false;
                }

                return true;
            }).WithMessage("This calendar does not exist.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.ColorHex).NotEmpty().WithMessage("ColorHex cannot be empty");
            RuleFor(x => x.ColorHex).Matches(ColorValidationHelper.HEX_RGB_REGEX).WithMessage("ColorHex must be in HEX.");
        }
    }
}
