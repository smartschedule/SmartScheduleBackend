namespace SmartSchedule.Application.User.Commands.UpdateUser
{
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Persistence;

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator(SmartScheduleDbContext context)
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty().MustAsync(async (request, val, token) =>
            {
                var userResult = await context.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (userResult == null || userResult.Email.Equals(val)) 
                {
                    return true;
                }

                return false;
            }).WithMessage("This email is already in use.");
            RuleFor(x => x.Password).MinimumLength(6).NotEmpty();
        }
    }
}
