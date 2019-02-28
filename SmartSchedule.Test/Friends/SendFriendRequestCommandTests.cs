namespace SmartSchedule.Test.Friends
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Friends.Commands.SendFriendRequest;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class SendFriendRequestCommandTests
    {
        private readonly SmartScheduleDbContext _context;
        public SendFriendRequestCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task SendValidFriendRequestShouldCreateRecordInDBContext()
        {
            var command = new SendFriendRequestCommand
            {
                UserId = 3,
                FriendId = 2
            };

            var commandHandler = new SendFriendRequestCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(command.FriendId) && x.SecoundUserId.Equals(command.UserId))
                                                                            || (x.FirstUserId.Equals(command.UserId) && x.SecoundUserId.Equals(command.FriendId)));

            friendRequest.ShouldNotBeNull();
            friendRequest.Type.ShouldBe(Domain.Enums.FriendshipTypes.pending_first_secound);
        }

        [Fact]
        public async Task SendInvalidFriendRequestShouldThrowNotFoundException()
        {
            var command = new SendFriendRequestCommand
            {
                FriendId = 22,
                UserId = 3
            };

            var commandHandler = new SendFriendRequestCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task SendSecoundTimeSameFriendRequestShouldThrowValidationException()
        {
            var command = new SendFriendRequestCommand
            {
                FriendId = 4,
                UserId = 3
            };

            var commandHandler = new SendFriendRequestCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task SendFriendRequestToUserWhoInvitedYouFirstShouldThrowValidationException()
        {
            var command = new SendFriendRequestCommand
            {
                FriendId = 3,
                UserId = 4
            };

            var commandHandler = new SendFriendRequestCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
