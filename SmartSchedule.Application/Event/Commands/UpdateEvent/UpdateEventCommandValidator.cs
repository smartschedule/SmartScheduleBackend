namespace SmartSchedule.Application.Event.Commands.UpdateEvent
{
    using FluentValidation;
    using SmartSchedule.Application.Event.Models;
    using SmartSchedule.Persistence;

    public class UpdateEventCommandValidator : AbstractValidator<EventDetailModel>
    {
        //TODO unify UpdateEventCommandValidator and CreateEventCommandValidator
        public UpdateEventCommandValidator(SmartScheduleDbContext context)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("You must set a start date");
            RuleFor(x => x.Duration).NotEmpty().WithMessage("You must set a duration");
            RuleFor(x => x.Latitude).NotEmpty().WithMessage("You must declare a latitude");
            RuleFor(x => x.Longitude).NotEmpty().WithMessage("You must declare a longitude");
            RuleFor(x => x.RepeatsEvery).NotEmpty().WithMessage("You must declare how often event repeats");
        }
    }
}
