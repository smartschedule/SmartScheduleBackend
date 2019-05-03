namespace SmartSchedule.Application.Friends.Commands.AcceptFriendRequest
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Persistence;

    public class AcceptFriendRequestCommand : AcceptOrRejectFriendRequestRequest, IRequest
    {
        public class Handler : IRequestHandler<AcceptFriendRequestCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
            {
                var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => ((x.FirstUserId.Equals(request.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(request.RequestedUserId)
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_secound))
                                                                                || (x.FirstUserId.Equals(request.RequestedUserId)
                                                                                && x.SecoundUserId.Equals(request.RequestingUserId))
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_secound_first)), cancellationToken);

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
