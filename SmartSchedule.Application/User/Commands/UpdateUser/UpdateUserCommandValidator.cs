namespace SmartSchedule.Application.User.Commands.UpdateUser
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserRequest>
    {
        private const int MIN_PASSWORD_LENGTH = 8;
        private const int MIN_USERNAME_LENGTH = 3;

        public UpdateUserCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("You must set username");
            RuleFor(x => x.UserName).MinimumLength(MIN_USERNAME_LENGTH).WithMessage("Username must have 3 or more characters");

            RuleFor(x => x.Email).NotEmpty().WithMessage("You must set Email");
            RuleFor(x => x.Email).EmailAddress().MustAsync(async (request, val, token) =>
            {
                var userResult = await uow.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (userResult == null || userResult.Email.Equals(val))
                {
                    return true;
                }

                return false;
            }).WithMessage("This email is already in use.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("You must set password"); ;
            RuleFor(x => x.Password).MinimumLength(MIN_PASSWORD_LENGTH).WithMessage("Password must have 3 or more characters");
        }
    }
}
