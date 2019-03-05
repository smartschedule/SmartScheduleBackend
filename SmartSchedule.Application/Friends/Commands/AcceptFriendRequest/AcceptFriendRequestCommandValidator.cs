namespace SmartSchedule.Application.Friends.Commands.AcceptFriendRequest
{
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Persistence;

    public class AcceptFriendRequestCommandValidator : AbstractValidator<AcceptFriendRequestCommand>
    {
        public AcceptFriendRequestCommandValidator(Domain.Entities.Friends friendRequest)
        {
            
            RuleFor(x => x.RequestingUserId).NotEmpty().Must((request, val, token) =>
            {
                if (friendRequest != null && friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_secound))
                {
                    return true;
                }

                return false;
            }).WithMessage("Friend request from that user doesn't exists");
            RuleFor(x => x.RequestedUserId).NotEmpty().Must((request, val, token) =>
            {
                if (friendRequest != null && friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.friends))
                {
                    return false;
                }

                return true;
            }).WithMessage("You are already friends");
        }
    }
}
