namespace SmartSchedule.Application.Friends.Commands.RejectFriendRequest
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class RejectFriendRequestCommand : IRequest
    {
        public AcceptOrRejectFriendInvitationRequest Data { get; set; }

        public RejectFriendRequestCommand()
        {

        }

        public RejectFriendRequestCommand(AcceptOrRejectFriendInvitationRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<RejectFriendRequestCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(RejectFriendRequestCommand request, CancellationToken cancellationToken)
            {
                AcceptOrRejectFriendInvitationRequest data = request.Data;

                var vResult = await new RejectFriendRequestCommandValidator(_uow).ValidateAsync(data, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                var friendRequest = await _uow.FriendsRepository.FirstOrDefaultAsync(x => ((x.FirstUserId.Equals(data.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(data.RequestedUserId)
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_second))
                                                                                || (x.FirstUserId.Equals(data.RequestedUserId)
                                                                                && x.SecoundUserId.Equals(data.RequestingUserId))
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_second_first)));
                _uow.FriendsRepository.Remove(friendRequest);
                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
