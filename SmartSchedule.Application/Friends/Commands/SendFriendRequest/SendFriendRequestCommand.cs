namespace SmartSchedule.Application.Friends.Commands.SendFriendRequest
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Application.DTO.Friends.Commands;

    public class SendFriendRequestCommand : SendFriendRequestRequest, IRequest
    {
        public class Handler : IRequestHandler<SendFriendRequestCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
            {
                //TODO: Refactor XD
                var friend = await _context.Users.FindAsync(request.FriendId);
                if (friend == null)
                {
                    throw new NotFoundException("User", request.FriendId);
                }

                var blockedList = await _context.Friends.Where(x => (x.FirstUserId.Equals(request.UserId) && x.SecoundUserId.Equals(request.FriendId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_first_secound)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both)))
                                                         || (x.SecoundUserId.Equals(request.UserId) && x.FirstUserId.Equals(request.FriendId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_scound_first)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both))))
                                                         .ToListAsync(cancellationToken);
                if (blockedList.Count != 0)
                {
                    throw new FluentValidation.ValidationException("The User whose you want to invite to friends is blocked or you are blocked by him!");
                }

                var vResult = await new SendFriendRequestCommandValidator(_context).ValidateAsync(request, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }
                var entity = new Domain.Entities.Friends
                {
                    FirstUserId = request.UserId,
                    SecoundUserId = request.FriendId,
                    Type = Domain.Enums.FriendshipTypes.pending_first_secound
                };

                _context.Friends.Add(entity);
                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
