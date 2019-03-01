namespace SmartSchedule.Test.Friends
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Friends.Commands.RemoveFriendRequest;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class RemoveFriendCommandTests
    {
        private readonly SmartScheduleDbContext _context;
        public RemoveFriendCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task Valid_Remove_Friend_Request_With_First_User_Should_Remove_Friend_Record_From_DB_Context()
        {
            var command = new RemoveFriendCommand
            {
                UserId = 5,
                FriendId = 4
            };

            var commandHandler = new RemoveFriendCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(command.UserId)
                                                                                && x.SecoundUserId.Equals(command.FriendId))
                                                                                || (x.FirstUserId.Equals(command.FriendId)
                                                                                && x.SecoundUserId.Equals(command.UserId)));

            friendRequest.ShouldBeNull();
        }

        [Fact]
        public async Task Valid_Remove_Friend_Request_With_Secound_User_Should_Remove_Friend_Record_From_DB_Context()
        {
            var command = new RemoveFriendCommand
            {
                UserId = 3,
                FriendId = 5
            };

            var commandHandler = new RemoveFriendCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(command.UserId)
                                                                                && x.SecoundUserId.Equals(command.FriendId))
                                                                                || (x.FirstUserId.Equals(command.FriendId)
                                                                                && x.SecoundUserId.Equals(command.UserId)));

            friendRequest.ShouldBeNull();
        }

        [Fact]
        public async Task Invalid_Remove_Friend_Request_Should_Throw_NotFoundException()
        {
            var command = new RemoveFriendCommand
            {
                UserId = 3,
                FriendId = 234
            };

            var commandHandler = new RemoveFriendCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }

    }
}
