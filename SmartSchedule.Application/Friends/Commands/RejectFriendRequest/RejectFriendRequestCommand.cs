namespace SmartSchedule.Application.Friends.Commands.RejectFriendRequest
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Persistence;

    public class RejectFriendRequestCommand : IRequest
    {
        public int RequestingUserId { get; set; }
        public int RequestedUserId { get; set; }
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

                var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => x.FirstUserId.Equals(request.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(request.RequestedUserId));
                _context.Remove(friendRequest);
                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
