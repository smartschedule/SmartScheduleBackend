namespace SmartSchedule.Application.Friends.Commands.SendFriendInvitation
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class SendFriendInvitationCommandValidator : AbstractValidator<SendFriendInvitationRequest>
    {
        public SendFriendInvitationCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.FriendId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                var friendRequest = await uow.FriendsRepository.FirstOrDefaultAsync(x =>
                                        (x.FirstUserId.Equals(val) && x.SecoundUserId.Equals(request.UserId)));

                if (friendRequest == null)
                {
                    return true;
                }

                return false;
            }).WithMessage("This user has already been requested to friends by second user");

            RuleFor(x => x.UserId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                var friendRequest = await uow.FriendsRepository.FirstOrDefaultAsync(x =>
                                         (x.FirstUserId.Equals(val) && x.SecoundUserId.Equals(request.FriendId)));

                if (friendRequest == null)
                {
                    return true;
                }

                return false;
            }).WithMessage("Request has already been sended");

        }
    }
}
