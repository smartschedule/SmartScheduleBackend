namespace SmartSchedule.Test.Friends
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using Shouldly;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Friends.Commands.RejectFriendRequest;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class RejectFriendRequestCommandTests
    {
        private readonly IUnitOfWork _uow;

        public RejectFriendRequestCommandTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task Reject_Valid_Friend_Request_Should_Remove_Friend_Request_From_DB_Context()
        {
            var requestData = new AcceptOrRejectFriendInvitationRequest
            {
                RequestingUserId = 3,
                RequestedUserId = 2
            };
            var command = new RejectFriendRequestCommand(requestData);

            var commandHandler = new RejectFriendRequestCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _uow.FriendsRepository.FirstOrDefaultAsync(x => x.FirstUserId.Equals(requestData.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(requestData.RequestedUserId));

            friendRequest.ShouldBeNull();
        }

        [Fact]
        public async Task Trying_To_Reject_Friend_Request_Which_Does_Not_Exists_Should_Throw_ValidationException()
        {
            var requestData = new AcceptOrRejectFriendInvitationRequest
            {
                RequestingUserId = 4,
                RequestedUserId = 34
            };
            var command = new RejectFriendRequestCommand(requestData);

            var commandHandler = new RejectFriendRequestCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<ValidationException>();
        }
    }
}
