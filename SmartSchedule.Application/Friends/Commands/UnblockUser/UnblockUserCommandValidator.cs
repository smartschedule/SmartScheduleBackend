namespace SmartSchedule.Application.Friends.Commands.UnblockUser
{
    using FluentValidation;
    using SmartSchedule.Application.Interfaces.UoW;

    public class UnblockUserCommandValidator : AbstractValidator<UnblockUserCommand>
    {
        public UnblockUserCommandValidator(IUnitOfWork context)
        {

        }
    }
}
