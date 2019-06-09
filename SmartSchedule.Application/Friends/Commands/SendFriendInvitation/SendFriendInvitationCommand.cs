namespace SmartSchedule.Application.Friends.Commands.SendFriendInvitation
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Exceptions;

    public class SendFriendInvitationCommand : IRequest
    {
        public SendFriendInvitationRequest Data { get; set; }

        public SendFriendInvitationCommand(SendFriendInvitationRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<SendFriendInvitationCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(SendFriendInvitationCommand request, CancellationToken cancellationToken)
            {
                SendFriendInvitationRequest data = request.Data;


                //TODO: Refactor XD
                var friend = await _uow.UsersRepository.GetByIdAsync(data.FriendId);
                if (friend == null)
                {
                    throw new NotFoundException("User", data.FriendId);
                }

                var blockedList = await _uow.FriendsRepository.GetBlockedFriends(data.UserId, data.FriendId, cancellationToken);
                if (blockedList.Count != 0)
                {
                    throw new FluentValidation.ValidationException("The User whose you want to invite to friends is blocked or you are blocked by him!");
                }

                var vResult = await new SendFriendInvitationCommandValidator(_uow).ValidateAsync(data, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }
                var entity = new Domain.Entities.Friends
                {
                    FirstUserId = data.UserId,
                    SecoundUserId = data.FriendId,
                    Type = Domain.Enums.FriendshipTypes.pending_first_second
                };

                _uow.FriendsRepository.Add(entity);
                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
