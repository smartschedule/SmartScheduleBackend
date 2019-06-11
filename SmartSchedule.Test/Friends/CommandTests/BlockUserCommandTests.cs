namespace SmartSchedule.Test.Friends
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Friends.Commands.BlockUser;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class BlockUserCommandTests
    {
        private readonly IUnitOfWork _uow;

        public BlockUserCommandTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task Valid_Block_User_Request_Should_Add_Block_Record_To_DB_Context()
        {
            var requestData = new BlockUserRequest
            {
                UserId = 4,
                UserToBlock = 2
            };
            var command = new BlockUserCommand(requestData);

            var commandHandler = new BlockUserCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _uow.FriendsRepository.FirstOrDefaultAsync(x => x.FirstUserId.Equals(requestData.UserId)
                                                                                && x.SecoundUserId.Equals(requestData.UserToBlock));

            friendRequest.ShouldNotBeNull();
            friendRequest.Type.ShouldBe(Domain.Enums.FriendshipTypes.block_first_second);
        }

        [Fact]
        public async Task Block_User_Which_Does_Not_Exists_Should_Throw_NotFoundException()
        {
            var requestData = new BlockUserRequest
            {
                UserId = 4,
                UserToBlock = 2342
            };
            var command = new BlockUserCommand(requestData);

            var commandHandler = new BlockUserCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }
    }
}
