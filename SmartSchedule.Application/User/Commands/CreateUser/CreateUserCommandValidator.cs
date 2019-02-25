namespace SmartSchedule.Application.User.Commands.CreateUser
{
    using FluentValidation;
    using SmartSchedule.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(SmartScheduleDbContext context)
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty().MustAsync(async (request, val , token) =>
            {
                var userResult = await context.Users.FirstOrDefaultAsync(x => x.Email.Equals(val));
                
                if(userResult == null)
                {
                    return true;
                }

                return false;
            }).WithMessage("This email is already in use.");
            RuleFor(x => x.Password).MinimumLength(6).NotEmpty();
        }
    }
}
