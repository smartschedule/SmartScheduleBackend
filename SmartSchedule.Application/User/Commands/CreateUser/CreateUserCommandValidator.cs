namespace SmartSchedule.Application.User.Commands.CreateUser
{
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Persistence;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserRequest>
    {
        private const int MIN_PASSWORD_LENGTH = 8;

        public CreateUserCommandValidator(SmartScheduleDbContext context)
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("You must set username");
            RuleFor(x => x.Email).EmailAddress().NotEmpty().MustAsync(async (request, val, token) =>
            {
                var userResult = await context.Users.FirstOrDefaultAsync(x => x.Email.Equals(val));

                if (userResult == null)
                {
                    return true;
                }

                return false;
            }).WithMessage("This email is already in use.");
            RuleFor(x => x.Password).MinimumLength(MIN_PASSWORD_LENGTH).NotEmpty();
        }
    }
}
