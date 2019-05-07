﻿namespace SmartSchedule.Application.Friends.Commands.RemoveFriend
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class RemoveFriendCommand : RemoveFriendRequest, IRequest
    {
        public class Handler : IRequestHandler<RemoveFriendCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
            {
                var vResult = await new RemoveFriendCommandValidator(_context).ValidateAsync(request, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => ((x.FirstUserId.Equals(request.UserId)
                                                                                && x.SecoundUserId.Equals(request.FriendId))
                                                                                || (x.FirstUserId.Equals(request.FriendId)
                                                                                && x.SecoundUserId.Equals(request.UserId)))
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.friends));
                if (friendRequest == null)
                {
                    throw new NotFoundException("FriendRequest", request.FriendId);
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
