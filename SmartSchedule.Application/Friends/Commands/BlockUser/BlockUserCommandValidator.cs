namespace SmartSchedule.Application.Friends.Commands.BlockUser
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class BlockUserCommandValidator : AbstractValidator<BlockUserRequest>
    {
        public BlockUserCommandValidator(IUnitOfWork uow)
        {

        }
    }
}
