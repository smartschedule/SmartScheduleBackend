namespace SmartSchedule.Application.Friends.Commands.AcceptFriendInvitation
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Friends.Commands;

    public class AcceptFriendInvitationCommand : IRequest
    {
        public AcceptOrRejectFriendInvitationRequest Data { get; set; }

        public AcceptFriendInvitationCommand()
        {

        }

        public AcceptFriendInvitationCommand(AcceptOrRejectFriendInvitationRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<AcceptFriendInvitationCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }
            public async Task<Unit> Handle(AcceptFriendInvitationCommand request, CancellationToken cancellationToken)
            {
                AcceptOrRejectFriendInvitationRequest data = request.Data;

                var friendRequest = await _uow.FriendsRepository.FirstOrDefaultAsync(x => ((x.FirstUserId.Equals(data.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(data.RequestedUserId)
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_second))
                                                                                || (x.FirstUserId.Equals(data.RequestedUserId)
                                                                                && x.SecoundUserId.Equals(data.RequestingUserId))
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_second_first)), cancellationToken);

                var vResult = new AcceptFriendInvitationCommandValidator(friendRequest).Validate(data);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                friendRequest.Type = Domain.Enums.FriendshipTypes.friends;
                _uow.FriendsRepository.Update(friendRequest);

                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
