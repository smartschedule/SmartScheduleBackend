namespace SmartSchedule.Application.Friends.Commands.BlockUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class BlockUserCommand : IRequest
    {
        public int FriendId { get; set; }
        public int UserId { get; set; }
        public class Handler : IRequestHandler<BlockUserCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(BlockUserCommand request, CancellationToken cancellationToken)
            {
                var vResult = await new BlockUserCommandValidator().ValidateAsync(request, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                var friend = await _context.Users.FindAsync(request.FriendId);
                if (friend == null)
                {
                    throw new NotFoundException("User", request.FriendId);
                }

                var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => x.FirstUserId.Equals(request.UserId)
                                                                                && x.SecoundUserId.Equals(request.FriendId));
                if (friendRequest != null)
                {
                    friendRequest.Type = Domain.Enums.FriendshipTypes.block_first_secound;
                    _context.Friends.Update(friendRequest);
                }
                else
                {
                    var entity = new Domain.Entities.Friends
                    {
                        FirstUserId = request.UserId,
                        SecoundUserId = request.FriendId,
                        Type = Domain.Enums.FriendshipTypes.block_scound_first
                    };
                }
                
                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
