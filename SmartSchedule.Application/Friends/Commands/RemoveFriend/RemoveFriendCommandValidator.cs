
namespace SmartSchedule.Application.Friends.Commands.RemoveFriend
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class RemoveFriendCommandValidator : AbstractValidator<RemoveFriendRequest>
    {
        public RemoveFriendCommandValidator(IUnitOfWork uow)
        {

        }
    }
}
