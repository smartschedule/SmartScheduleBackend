namespace SmartSchedule.Test.Friends
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Friends.Commands.RemoveFriend;
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
            var requestData = new RemoveFriendRequest
            {
                UserId = 5,
                FriendId = 4
            };
            var command = new RemoveFriendCommand(requestData);
  
            var commandHandler = new RemoveFriendCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(requestData.UserId)
                                                                                && x.SecoundUserId.Equals(requestData.FriendId))
                                                                                || (x.FirstUserId.Equals(requestData.FriendId)
                                                                                && x.SecoundUserId.Equals(requestData.UserId)));

            friendRequest.ShouldBeNull();
        }

        [Fact]
        public async Task Valid_Remove_Friend_Request_With_Secound_User_Should_Remove_Friend_Record_From_DB_Context()
        {
            var requestData = new RemoveFriendRequest
            {
                UserId = 3,
                FriendId = 5
            };
            var command = new RemoveFriendCommand(requestData);

            var commandHandler = new RemoveFriendCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(requestData.UserId)
                                                                                && x.SecoundUserId.Equals(requestData.FriendId))
                                                                                || (x.FirstUserId.Equals(requestData.FriendId)
                                                                                && x.SecoundUserId.Equals(requestData.UserId)));

            friendRequest.ShouldBeNull();
        }

        [Fact]
        public async Task Invalid_Remove_Friend_Request_Should_Throw_NotFoundException()
        {
            var requestData = new RemoveFriendRequest
            {
                UserId = 3,
                FriendId = 234
            };
            var command = new RemoveFriendCommand(requestData);

            var commandHandler = new RemoveFriendCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }

    }
}
