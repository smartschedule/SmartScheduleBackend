namespace SmartSchedule.Application.Friends.Commands.RejectFriendRequest
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Persistence;

    public class RejectFriendRequestCommand : AcceptOrRejectFriendRequestRequest, IRequest
    {
        public class Handler : IRequestHandler<RejectFriendRequestCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(RejectFriendRequestCommand request, CancellationToken cancellationToken)
            {
                var vResult = await new RejectFriendRequestCommandValidator(_context).ValidateAsync(request, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => ((x.FirstUserId.Equals(request.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(request.RequestedUserId)
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_secound))
                                                                                || (x.FirstUserId.Equals(request.RequestedUserId)
                                                                                && x.SecoundUserId.Equals(request.RequestingUserId))
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_secound_first)));
                _context.Remove(friendRequest);
                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
