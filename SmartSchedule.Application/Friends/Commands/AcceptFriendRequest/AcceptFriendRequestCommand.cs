namespace SmartSchedule.Application.Friends.Commands.AcceptFriendRequest
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Persistence;

    public class AcceptFriendRequestCommand : IRequest
    {
        public int RequestingUserId { get; set; }
        public int RequestedUserId { get; set; }
        public class Handler : IRequestHandler<AcceptFriendRequestCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
            {
                var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => x.FirstUserId.Equals(request.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(request.RequestedUserId), cancellationToken);

                var vResult = new AcceptFriendRequestCommandValidator(friendRequest).Validate(request);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                friendRequest.Type = Domain.Enums.FriendshipTypes.friends;
                _context.Update(friendRequest);

                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
