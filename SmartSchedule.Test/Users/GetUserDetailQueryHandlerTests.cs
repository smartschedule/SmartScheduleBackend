namespace SmartSchedule.Test.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.User.Queries.GetUserDetails;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class GetUserDetailQueryHandlerTests
    {
        private readonly SmartScheduleDbContext _context;

        public GetUserDetailQueryHandlerTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task GetUserDetail()
        {
            var sut = new GetUserDetailQueryHandler(_context);

            var result = await sut.Handle(new GetUserDetailQuery { Id = 2 }, CancellationToken.None);

            result.ShouldBeOfType<UserDetailModel>();
            result.Id.ShouldBe(2);
        }
    }
}
