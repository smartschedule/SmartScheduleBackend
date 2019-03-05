namespace SmartSchedule.Test.Friends
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Friends.Commands.BlockUser;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class BlockUserCommandTests
    {
        private readonly SmartScheduleDbContext _context;

        public BlockUserCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task Valid_Block_User_Request_Should_Add_Block_Record_To_DB_Context()
        {
            var command = new BlockUserCommand
            {
                UserId = 4,
                UserToBlock = 2
            };

            var commandHandler = new BlockUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => x.FirstUserId.Equals(command.UserId)
                                                                                && x.SecoundUserId.Equals(command.UserToBlock));

            friendRequest.ShouldNotBeNull();
            friendRequest.Type.ShouldBe(Domain.Enums.FriendshipTypes.block_first_secound);
        }

        [Fact]
        public async Task Block_User_Which_Does_Not_Exists_Should_Throw_NotFoundException()
        {
            var command = new BlockUserCommand
            {
                UserId = 4,
                UserToBlock = 2342
            };

            var commandHandler = new BlockUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }
    }
}
