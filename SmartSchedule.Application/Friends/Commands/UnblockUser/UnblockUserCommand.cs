namespace SmartSchedule.Application.Friends.Commands.UnblockUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Exceptions;

    public class UnblockUserCommand : IRequest
    {
        public UnblockUserRequest Data { get; set; }

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

                var friendRequest = await _uow.FriendsRepository.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(data.UserId)
                                                          && x.SecoundUserId.Equals(data.UserToUnblockId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_first_second)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both)))
                                                         || (x.SecoundUserId.Equals(data.UserId) && x.FirstUserId.Equals(data.UserToUnblockId)
                                                         && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_second_first)
                                                         || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both))));

                if (friendRequest == null)
                {
                    throw new NotFoundException("BlockedUser", data.UserToUnblockId);
                }
                else if (friendRequest.FirstUserId.Equals(data.UserId)
                    && friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_both))
                {
                    friendRequest.Type = Domain.Enums.FriendshipTypes.block_second_first;
                    _uow.FriendsRepository.Update(friendRequest);
                }
                else
                {
                    _uow.FriendsRepository.Remove(friendRequest);
                }

                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
