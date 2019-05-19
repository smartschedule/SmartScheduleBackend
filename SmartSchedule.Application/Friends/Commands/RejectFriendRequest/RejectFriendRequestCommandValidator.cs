namespace SmartSchedule.Application.Friends.Commands.RejectFriendRequest
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class RejectFriendRequestCommandValidator : AbstractValidator<AcceptOrRejectFriendInvitationRequest>
    {
        public RejectFriendRequestCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.RequestingUserId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                var friendRequest = await uow.FriendsRepository.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(request.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(request.RequestedUserId)
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_second))
                                                                                || (x.FirstUserId.Equals(request.RequestedUserId)
                                                                                && x.SecoundUserId.Equals(request.RequestingUserId)
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_second_first)));

                if (friendRequest != null)
                {
                    return true;
                }

                return false;
            }).WithMessage("Friend request from that user doesn't exists");
        }
    }
}
