namespace SmartSchedule.Application.User.Commands.CreateUser
{
    using FluentValidation;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.User.Commands;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserRequest>
    {
        private const int MIN_PASSWORD_LENGTH = 8;
        private const int MIN_USERNAME_LENGTH = 3;

        public CreateUserCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("You must set username");
            RuleFor(x => x.UserName).MinimumLength(MIN_USERNAME_LENGTH).WithMessage("Username must have 3 or more characters");

            RuleFor(x => x.Email).NotEmpty().WithMessage("You must set Email");
            RuleFor(x => x.Email).EmailAddress().MustAsync(async (request, val, token) =>
            {
                var userResult = await uow.UsersRepository.FirstOrDefaultAsync(x => x.Email.Equals(val));

                if (userResult == null)
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
