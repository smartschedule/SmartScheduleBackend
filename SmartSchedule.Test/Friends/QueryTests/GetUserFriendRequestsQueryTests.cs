namespace SmartSchedule.Test.Friends.QueryTests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Friends.Queries;
    using SmartSchedule.Application.Friends.Queries.GetUserFriendRequests;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class GetUserFriendRequestsQueryTests
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetUserFriendRequestsQueryTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetPendingUserFriendRequests()
        {
            var sut = new GetUserFriendRequestsQuery.Handler(_uow, _mapper);

            var result = await sut.Handle(new GetUserFriendRequestsQuery(new IdRequest(7)), CancellationToken.None);

            result.ShouldBeOfType<FriendsListResponse>();
            result.Users.Count.ShouldBe(3);
        }
    }
}
