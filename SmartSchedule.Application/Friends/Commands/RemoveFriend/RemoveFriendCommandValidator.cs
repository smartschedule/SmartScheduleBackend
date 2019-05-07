
namespace SmartSchedule.Application.Friends.Commands.RemoveFriend
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Persistence;

    public class RemoveFriendCommandValidator : AbstractValidator<RemoveFriendRequest>
    {
        public RemoveFriendCommandValidator(SmartScheduleDbContext context)
        {

        }
    }
}
