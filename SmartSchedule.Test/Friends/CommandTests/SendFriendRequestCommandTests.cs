namespace SmartSchedule.Test.Friends
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Friends.Commands.SendFriendInvitation;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class SendFriendRequestCommandTests
    {
        private readonly IUnitOfWork _uow;

        public SendFriendRequestCommandTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task SendValidFriendRequestShouldCreateRecordInDBContext()
        {
            var requestData = new SendFriendInvitationRequest
            {
                UserId = 3,
                FriendId = 2
            };
            var command = new SendFriendInvitationCommand(requestData);

            var commandHandler = new SendFriendInvitationCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _uow.FriendsRepository.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(requestData.FriendId) && x.SecoundUserId.Equals(requestData.UserId))
                                                                            || (x.FirstUserId.Equals(requestData.UserId) && x.SecoundUserId.Equals(requestData.FriendId)));

            friendRequest.ShouldNotBeNull();
            friendRequest.Type.ShouldBe(Domain.Enums.FriendshipTypes.pending_first_second);
        }

        [Fact]
        public async Task SendInvalidFriendRequestShouldThrowNotFoundException()
        {
            var requestData = new SendFriendInvitationRequest
            {
                FriendId = 22,
                UserId = 3
            };
            var command = new SendFriendInvitationCommand(requestData);

            var commandHandler = new SendFriendInvitationCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task SendSecoundTimeSameFriendRequestShouldThrowValidationException()
        {
            var requestData = new SendFriendInvitationRequest
            {
                FriendId = 4,
                UserId = 3
            };
            var command = new SendFriendInvitationCommand(requestData);

            var commandHandler = new SendFriendInvitationCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task SendFriendRequestToUserWhoInvitedYouFirstShouldThrowValidationException()
        {
            var requestData = new SendFriendInvitationRequest
            {
                FriendId = 3,
                UserId = 4
            };
            var command = new SendFriendInvitationCommand(requestData);

            var commandHandler = new SendFriendInvitationCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
