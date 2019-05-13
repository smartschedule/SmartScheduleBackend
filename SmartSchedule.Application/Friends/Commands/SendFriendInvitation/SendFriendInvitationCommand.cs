namespace SmartSchedule.Application.Friends.Commands.SendFriendInvitation
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces.UoW;

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
            private readonly IUnitOfWork _context;

            public Handler(IUnitOfWork context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(SendFriendInvitationCommand request, CancellationToken cancellationToken)
            {
                SendFriendInvitationRequest data = request.Data;

                //TODO: Refactor XD
                var friend = await _context.Users.FindAsync(data.FriendId);
                if (friend == null)
                {
                    throw new NotFoundException("User", data.FriendId);
                }

                var blockedList = await _context.Friends.Where(x => (x.FirstUserId.Equals(data.UserId) && x.SecoundUserId.Equals(data.FriendId)
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

                var vResult = await new SendFriendInvitationCommandValidator(_context).ValidateAsync(data, cancellationToken);
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

                _context.Friends.Add(entity);
                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
