namespace SmartSchedule.Application.Friends.Commands.AcceptFriendRequest
{
    using FluentValidation;
    using SmartSchedule.Persistence;

    public class AcceptFriendRequestCommandValidator : AbstractValidator<AcceptFriendRequestCommand>
    {
        public AcceptFriendRequestCommandValidator(SmartScheduleDbContext context)
        {
                            
        }
    }
}
