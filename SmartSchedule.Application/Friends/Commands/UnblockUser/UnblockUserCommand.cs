namespace SmartSchedule.Application.Friends.Commands.UnblockUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class UnblockUserCommand : IRequest
    {
        public UnblockUserRequest Data { get; set; }

        public UnblockUserCommand()
        {

        }

        public UnblockUserCommand(UnblockUserRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<UnblockUserCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
            {
                UnblockUserRequest data = request.Data;

                var friendRequest = await _uow.Friends.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(data.UserId)
                                                          && x.SecoundUserId.Equals(data.UserToUnblockId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_first_secound)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both)))
                                                         || (x.SecoundUserId.Equals(data.UserId) && x.FirstUserId.Equals(data.UserToUnblockId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_scound_first)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both))));

                if (friendRequest == null)
                {
                    throw new NotFoundException("BlockedUser", data.UserToUnblockId);
                }
                else if (friendRequest.FirstUserId.Equals(data.UserId)
                    && friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_both))
                {
                    friendRequest.Type = Domain.Enums.FriendshipTypes.block_scound_first;
                    _uow.Friends.Update(friendRequest);
                }
                else
                {
                    _uow.Friends.Remove(friendRequest);
                }

                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
