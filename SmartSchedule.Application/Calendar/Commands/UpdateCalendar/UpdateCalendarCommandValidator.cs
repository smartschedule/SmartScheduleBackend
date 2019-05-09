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
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.ColorHex).NotEmpty().WithMessage("ColorHex cannot be empty");
            RuleFor(x => x.ColorHex).Matches(ColorValidationHelper.HEX_RGB_REGEX).WithMessage("ColorHex must be in HEX.");
        }
    }
}
