using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using SmartSchedule.Application.Event.Queries.GetEventDetails;
using SmartSchedule.Persistence;
using SmartSchedule.Test.Infrastructure;
using Xunit;

namespace SmartSchedule.Test.Users
{
    [Collection("TestCollection")]
    public class GetEventDetailQueryHandlerTests
    {
        private readonly SmartScheduleDbContext _context;

        public GetEventDetailQueryHandlerTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task GetEventDetail()
        {
            var sut = new GetEventDetailQueryHandler(_context);

            var result = await sut.Handle(new GetEventDetailQuery { Id = 3 }, CancellationToken.None);

            result.ShouldBeOfType<EventDetailModel>();
            result.Id.ShouldBe(3);
        }
    }
}
