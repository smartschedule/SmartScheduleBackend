namespace SmartSchedule.Test.Friends.QueryTests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.Friends.Models;
    using SmartSchedule.Application.Friends.Queries.GetFriends;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class GetFriendsListQueryTests
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetFriendsListQueryTests(TestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetFriendsList()
        {
            var sut = new GetFriendsListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetFriendsListQuery { UserId = 6 }, CancellationToken.None);

            result.ShouldBeOfType<FriendsListViewModel>();
            result.Users.Count.ShouldBe(1);
        }
    }
}
