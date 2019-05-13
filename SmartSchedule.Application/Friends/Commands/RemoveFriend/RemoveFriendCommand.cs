namespace SmartSchedule.Application.Friends.Commands.RemoveFriend
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces.UoW;

    public class RemoveFriendCommand : IRequest
    {
        public RemoveFriendRequest Data { get; set; }

        public RemoveFriendCommand()
        {

        }

        public RemoveFriendCommand(RemoveFriendRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<RemoveFriendCommand, Unit>
        {
            private readonly IUnitOfWork _context;

            public Handler(IUnitOfWork context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
            {
                RemoveFriendRequest data = request.Data;

                var vResult = await new RemoveFriendCommandValidator(_context).ValidateAsync(data, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => ((x.FirstUserId.Equals(data.UserId)
                                                                                && x.SecoundUserId.Equals(data.FriendId))
                                                                                || (x.FirstUserId.Equals(data.FriendId)
                                                                                && x.SecoundUserId.Equals(data.UserId)))
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.friends));
                if (friendRequest == null)
                {
                    throw new NotFoundException("FriendRequest", data.FriendId);
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
