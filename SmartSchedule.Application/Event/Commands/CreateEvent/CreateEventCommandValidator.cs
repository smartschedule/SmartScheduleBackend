namespace SmartSchedule.Application.Event.Commands.CreateEvent
{
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Event.Models;
    using SmartSchedule.Persistence;

    public class CreateEventCommandValidator : AbstractValidator<EventCreateModel>
    {
        public CreateEventCommandValidator(SmartScheduleDbContext context)
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
            RuleFor(x => x.RepeatsEvery).NotEmpty().WithMessage("You must declare how often event repeats");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Latitude).NotEmpty().WithMessage("You must declare a latitude");
            RuleFor(x => x.Longitude).NotEmpty().WithMessage("You must declare a longitude");
        }
    }
}
