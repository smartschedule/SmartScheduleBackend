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
        public int UserToBlock { get; set; }
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

                var friend = await _context.Users.FindAsync(request.UserToBlock);
                if (friend == null)
                {
                    throw new NotFoundException("User", request.UserToBlock);
                }

                var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(request.UserId)
                                                                                && x.SecoundUserId.Equals(request.UserToBlock))
                                                                                || (x.FirstUserId.Equals(request.UserToBlock)
                                                                                && x.SecoundUserId.Equals(request.UserId)));
                if (friendRequest != null && (friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_first_secound)
                                              || friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_scound_first)))
                {
                    friendRequest.Type = Domain.Enums.FriendshipTypes.block_both;
                    _context.Friends.Update(friendRequest);
                }
                else if(friendRequest != null && !(friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_first_secound)
                                              || friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_scound_first)))
                {
                    friendRequest.Type = Domain.Enums.FriendshipTypes.block_first_secound;
                    _context.Friends.Update(friendRequest);
                }
                else
                {
                    var entity = new Domain.Entities.Friends
                    {
                        FirstUserId = request.UserId,
                        SecoundUserId = request.UserToBlock,
                        Type = Domain.Enums.FriendshipTypes.block_scound_first
                    };
                }
                
                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
