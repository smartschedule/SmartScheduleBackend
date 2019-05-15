namespace SmartSchedule.Application.Friends.Commands.SendFriendInvitation
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class SendFriendInvitationCommand : IRequest
    {
        public SendFriendInvitationRequest Data { get; set; }

        public SendFriendInvitationCommand()
        {

        }

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
                var friend = await _uow.Users.FindAsync(data.FriendId);
                if (friend == null)
                {
                    throw new NotFoundException("User", data.FriendId);
                }

                var blockedList = await _uow.Friends.Where(x => (x.FirstUserId.Equals(data.UserId) && x.SecoundUserId.Equals(data.FriendId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_first_secound)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both)))
                                                         || (x.SecoundUserId.Equals(data.UserId) && x.FirstUserId.Equals(data.FriendId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_scound_first)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both))))
                                                         .ToListAsync(cancellationToken);
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
                    Type = Domain.Enums.FriendshipTypes.pending_first_secound
                };

                _uow.Friends.Add(entity);
                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
