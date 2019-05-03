namespace SmartSchedule.Test.Friends.QueryTests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.DTO.Friends.Queries;
    using SmartSchedule.Application.Friends.Queries.GetUserFriendRequests;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class GetUserFriendRequestsQueryTests
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetUserFriendRequestsQueryTests(TestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetPendingUserFriendRequests()
        {
            var sut = new GetUserFriendRequestsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetUserFriendRequestsQuery { UserId = 7 }, CancellationToken.None);

            result.ShouldBeOfType<FriendsListResponse>();
            result.Users.Count.ShouldBe(3);
        }
    }
}
