namespace SmartSchedule.Test.Friends.QueryTests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.DTO.Friends.Queries;
    using SmartSchedule.Application.Friends.Queries.GetPendingUserFriendRequests;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class GetPendingUserFriendRequestsQueryTests
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetPendingUserFriendRequestsQueryTests(TestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetPendingUserFriendRequests()
        {
            var sut = new GetPendingUserFriendRequestsQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPendingUserFriendRequestsQuery { UserId = 7 }, CancellationToken.None);

            result.ShouldBeOfType<FriendsListResponse>();
            result.Users.Count.ShouldBe(2);
        }
    }
}
