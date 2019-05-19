namespace SmartSchedule.Application.Friends.Commands.AcceptFriendInvitation
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.Friends.Commands;

    public class AcceptFriendInvitationCommandValidator : AbstractValidator<AcceptOrRejectFriendInvitationRequest>
    {
        //ToDo change friendRequest to uow - IUnitOfWork
        public AcceptFriendInvitationCommandValidator(Domain.Entities.Friends friendRequest)
        {
            RuleFor(x => x.RequestingUserId).NotEmpty().Must((request, val, token) =>
            {
                if (friendRequest != null && friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_second))
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
