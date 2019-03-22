namespace SmartSchedule.Application.Calendar.Commands.UpdateCalendar
{
    using FluentValidation;
    using SmartSchedule.Persistence;

    public class UpdateCalendarCommandValidator : AbstractValidator<UpdateCalendarCommand>
    {
        public UpdateCalendarCommandValidator(SmartScheduleDbContext context)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.ColorHex).NotEmpty().WithMessage("You must declare a color");
        }
    }
}
