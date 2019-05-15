namespace SmartSchedule.Application.Friends.Commands.UnblockUser
{
    using FluentValidation;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class UnblockUserCommandValidator : AbstractValidator<UnblockUserCommand>
    {
        public UnblockUserCommandValidator(IUnitOfWork uow)
        {

        }
    }
}
