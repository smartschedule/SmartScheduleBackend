namespace SmartSchedule.Application.User.Commands.CreateUser
{
    using FluentValidation;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).MinimumLength(6).NotEmpty();
        }
    }
}
