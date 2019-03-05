namespace SmartSchedule.Test.Friends
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Friends.Commands.RejectFriendRequest;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class RejectFriendRequestCommandTests
    {
        private readonly SmartScheduleDbContext _context;
        public RejectFriendRequestCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task Reject_Valid_Friend_Request_Should_Remove_Friend_Request_From_DB_Context()
        {
            var command = new RejectFriendRequestCommand
            {
                RequestingUserId = 3,
                RequestedUserId = 2
            };

            var commandHandler = new RejectFriendRequestCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => x.FirstUserId.Equals(command.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(command.RequestedUserId));

            friendRequest.ShouldBeNull();
        }

        [Fact]
        public async Task Trying_To_Reject_Friend_Request_Which_Does_Not_Exists_Should_Throw_ValidationException()
        {
            var command = new RejectFriendRequestCommand
            {
                RequestingUserId = 4,
                RequestedUserId = 34
            };

            var commandHandler = new RejectFriendRequestCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<ValidationException>();
        }
    }
}
