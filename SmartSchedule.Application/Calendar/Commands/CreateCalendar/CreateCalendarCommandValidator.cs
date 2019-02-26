namespace SmartSchedule.Application.Calendar.Commands.CreateCalendar
{
    using FluentValidation;
    using SmartSchedule.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class CreateCalendarCommandValidator : AbstractValidator<CreateCalendarCommand>
    {
        public CreateCalendarCommandValidator(SmartScheduleDbContext context)
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
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.ColorHex).NotEmpty().WithMessage("You must declare a color");
        }
    }
}
