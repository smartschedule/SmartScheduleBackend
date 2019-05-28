namespace SmartSchedule.Test.Friends
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Friends.Commands.AcceptFriendInvitation;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class AcceptFriendRequestCommandTests
    {
        private readonly IUnitOfWork _uow;

        public AcceptFriendRequestCommandTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task Trying_To_Accept_Friend_Request_Which_Does_Not_Exists_Should_Throw_ValidationException()
        {
            var requestData = new AcceptOrRejectFriendInvitationRequest
            {
                RequestingUserId = 22,
                RequestedUserId = 2
            };
            var command = new AcceptFriendInvitationCommand(requestData);

            var commandHandler = new AcceptFriendInvitationCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None)
                .ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task Accept_Valid_Friend_Request_Should_Change_Type_In_DB_Context()
        {
            var requestData = new AcceptOrRejectFriendInvitationRequest
            {
                RequestingUserId = 3,
                RequestedUserId = 4
            };
            var command = new AcceptFriendInvitationCommand(requestData);

            var commandHandler = new AcceptFriendInvitationCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var friendRequest = await _uow.FriendsRepository.FirstOrDefaultAsync(x => x.FirstUserId.Equals(requestData.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(requestData.RequestedUserId));

            friendRequest.ShouldNotBeNull();
            friendRequest.Type.ShouldBe(Domain.Enums.FriendshipTypes.friends);
        }
    }
}
