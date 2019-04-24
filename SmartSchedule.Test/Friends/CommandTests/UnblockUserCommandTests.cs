namespace SmartSchedule.Test.Friends
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Friends.Commands.UnblockUser;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class UnblockUserCommandTests
    {
        private readonly SmartScheduleDbContext _context;

        public UnblockUserCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task Valid_unBlock_User_Request_Should_Remove_Block_Record_To_DB_Context()
        {
            var command = new UnblockUserCommand
            {
                UserId = 4,
                UserToUnblockId = 2
            };

            var commandHandler = new UnblockUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => x.FirstUserId.Equals(command.UserId)
                                                                                && x.SecoundUserId.Equals(command.UserToUnblockId));

            friendRequest.ShouldBeNull();
        }

        [Fact]
        public async Task Unblock_User_Which_Does_Not_Exists_Should_Throw_NotFoundException()
        {
            var command = new UnblockUserCommand
            {
                UserId = 4,
                UserToUnblockId = 222
            };

            var commandHandler = new UnblockUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }
    }
}
