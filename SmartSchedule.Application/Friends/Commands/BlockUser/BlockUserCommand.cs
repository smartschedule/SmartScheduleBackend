namespace SmartSchedule.Application.Friends.Commands.BlockUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class BlockUserCommand : IRequest
    {
        public BlockUserRequest Data { get; set; }

        public BlockUserCommand()
        {

        }

        public BlockUserCommand(BlockUserRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<BlockUserCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(BlockUserCommand request, CancellationToken cancellationToken)
            {
                BlockUserRequest data = request.Data;

                var vResult = await new BlockUserCommandValidator(_uow).ValidateAsync(data, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                var friend = await _uow.UsersRepository.GetByIdAsync(data.UserToBlock);
                if (friend == null)
                {
                    throw new NotFoundException("User", data.UserToBlock);
                }

                var friendRequest = await _uow.FriendsRepository.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(data.UserId)
                                                                                && x.SecoundUserId.Equals(data.UserToBlock))
                                                                                || (x.FirstUserId.Equals(data.UserToBlock)
                                                                                && x.SecoundUserId.Equals(data.UserId)));

                if (friendRequest != null && (friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_first_second)
                                              || friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_second_first)))
                {
                    friendRequest.Type = Domain.Enums.FriendshipTypes.block_both;
                    _uow.FriendsRepository.Update(friendRequest);
                }
                else if (friendRequest != null && !(friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_first_second)
                                              || friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.block_second_first)))
                {
                    friendRequest.Type = Domain.Enums.FriendshipTypes.block_first_second;
                    _uow.FriendsRepository.Update(friendRequest);
                }
                else
                {
                    new Domain.Entities.Friends
                    {
                        FirstUserId = data.UserId,
                        SecoundUserId = data.UserToBlock,
                        Type = Domain.Enums.FriendshipTypes.block_second_first
                    };
                }

                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
