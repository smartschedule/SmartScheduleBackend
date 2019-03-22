namespace SmartSchedule.Test.Calendars
{
    using Shouldly;
    using SmartSchedule.Application.Calendar.Queries.GetCalendarDetails;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    [Collection("TestCollection")]
    public class GetCalendarDetailQueryHandlerTests
    {
        private readonly SmartScheduleDbContext _context;

        public GetCalendarDetailQueryHandlerTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task GetCalendarDetail()
        {
            var sut = new GetCalendarDetailQueryHandler(_context);

            var result = await sut.Handle(new GetCalendarDetailQuery { Id = 2 }, CancellationToken.None);

            result.ShouldBeOfType<CalendarDetailModel>();
            result.Id.ShouldBe(2);
            result.Events.ShouldBeOfType<List<EventLookupModel>>();
        }
    }
}
