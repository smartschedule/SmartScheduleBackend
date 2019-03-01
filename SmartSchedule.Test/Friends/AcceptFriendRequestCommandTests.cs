namespace SmartSchedule.Test.Friends
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Friends.Commands.AcceptFriendRequest;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class AcceptFriendRequestCommandTests
    {
        private readonly SmartScheduleDbContext _context;

        public AcceptFriendRequestCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task Accept_Valid_Friend_Request_Should_Change_Type_In_DB_Context()
        {
            var command = new AcceptFriendRequestCommand
            {
                RequestingUserId = 3,
                RequestedUserId = 4
            };

            var commandHandler = new AcceptFriendRequestCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => x.FirstUserId.Equals(command.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(command.RequestedUserId));

            friendRequest.ShouldNotBeNull();
            friendRequest.Type.ShouldBe(Domain.Enums.FriendshipTypes.friends);
        }

        [Fact]
        public async Task Trying_To_Accept_Friend_Request_Which_Does_Not_Exists_Should_Throw_ValidationException()
        {
            var command = new AcceptFriendRequestCommand
            {
                RequestingUserId = 22,
                RequestedUserId = 2
            };

            var commandHandler = new AcceptFriendRequestCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None)
                .ShouldThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
