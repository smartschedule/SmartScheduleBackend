namespace SmartSchedule.Application.Friends.Commands.SendFriendRequest
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class SendFriendRequestCommand : IRequest
    {
        public int FriendId { get; set; }
        public int UserId { get; set; }

        public class Handler : IRequestHandler<SendFriendRequestCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
            {
                var vResult = await new SendFriendRequestCommandValidator(_context).ValidateAsync(request, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                var friend = await _context.Users.FindAsync(request.FriendId);
                if(friend == null)
                {
                    throw new NotFoundException("User", request.FriendId);
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
