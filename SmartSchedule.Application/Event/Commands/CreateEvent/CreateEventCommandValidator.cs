namespace SmartSchedule.Application.Event.Commands.CreateEvent
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Application.Interfaces.UoW;

    public class CreateEventCommandValidator : AbstractValidator<CreateEventRequest>
    {
        public CreateEventCommandValidator(IUnitOfWork context)
        {
            RuleFor(x => x.CalendarId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                var userResult = await context.Calendars.FirstOrDefaultAsync(x => x.Id.Equals(val));

                if (userResult == null)
                {
                    return false;
                }

                return true;
            }).WithMessage("This calendar does not exist.");

            RuleFor(x => x.StartDate).NotEmpty().WithMessage("You must set a start date");
            RuleFor(x => x.Duration).NotEmpty().WithMessage("You must set a duration");
            RuleFor(x => x.RepeatsTo).Must((request, val) =>
            {
                if (val == null || val > request.StartDate)
                    return true;

                return true;
            }).WithMessage("RepeatsTo must be greater than StartDate or equal null.");

            RuleFor(x => x.Type).IsInEnum().WithMessage("You must set a valid type of event");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Latitude).NotNull().InclusiveBetween(-90, 90).WithMessage("You must declare a valid latitude");
            RuleFor(x => x.Longitude).NotNull().InclusiveBetween(-90, 90).WithMessage("You must declare a valid longitude");

            RuleFor(x => x.ColorHex).NotEmpty().WithMessage("ColorHex cannot be empty");
            RuleFor(x => x.ColorHex).Matches(ColorValidationHelper.HEX_RGB_REGEX).WithMessage("ColorHex must be in HEX.");
        }
    }
}
