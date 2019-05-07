namespace SmartSchedule.Application.Calendar.Commands.CreateCalendar
{
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Persistence;

    public class CreateCalendarCommandValidator : AbstractValidator<CreateCalendarRequest>
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
            RuleFor(x => x.ColorHex).NotEmpty().WithMessage("ColorHex cannot be empty");
            RuleFor(x => x.ColorHex).Matches(ColorValidationHelper.HEX_RGB_REGEX).WithMessage("ColorHex must be in HEX.");
        }
    }
}
