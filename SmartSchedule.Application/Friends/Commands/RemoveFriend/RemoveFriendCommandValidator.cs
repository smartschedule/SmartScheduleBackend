
namespace SmartSchedule.Application.Friends.Commands.RemoveFriend
{
    using FluentValidation;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Friends.Commands;

    public class RemoveFriendCommandValidator : AbstractValidator<RemoveFriendRequest>
    {
        public RemoveFriendCommandValidator(IUnitOfWork uow)
        {

        }
    }
}
