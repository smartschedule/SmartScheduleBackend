namespace SmartSchedule.Application.Friends.Commands.UnblockUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class UnblockUserCommand : IRequest
    {
        public int UserId { get; set; }
        public int UserToUnblockId { get; set; }

        public class Handler : IRequestHandler<UnblockUserCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
            {
                var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(request.UserId)
                                                          && x.SecoundUserId.Equals(request.UserToUnblockId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_first_secound)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both)))
                                                         || (x.SecoundUserId.Equals(request.UserId) && x.FirstUserId.Equals(request.UserToUnblockId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_scound_first)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both))));

                if(friendRequest == null)
                {
                    throw new NotFoundException("BlockedUser", request.UserToUnblockId);
                }

                if (friendRequest != null && friendRequest.FirstUserId.Equals(request.UserId)
                    && friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_both))
                {
                    friendRequest.Type = Domain.Enums.FriendshipTypes.block_scound_first;
                    _context.Friends.Update(friendRequest);
                }
                else if (friendRequest != null && friendRequest.FirstUserId.Equals(request.UserId)
                    && friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_both))
                {
                    friendRequest.Type = Domain.Enums.FriendshipTypes.block_first_secound;
                    _context.Friends.Update(friendRequest);
                }
                else
                {
                    _context.Friends.Remove(friendRequest);
                }

                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
