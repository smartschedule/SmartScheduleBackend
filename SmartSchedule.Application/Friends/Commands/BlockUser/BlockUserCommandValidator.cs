namespace SmartSchedule.Application.Friends.Commands.BlockUser
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Interfaces.UoW;

    public class BlockUserCommandValidator : AbstractValidator<BlockUserRequest>
    {
        public BlockUserCommandValidator(IUnitOfWork uow)
        {

        }
    }
}
