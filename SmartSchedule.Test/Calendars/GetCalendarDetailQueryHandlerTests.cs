namespace SmartSchedule.Test.Calendars
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Queries.GetCalendarDetails;
    using SmartSchedule.Application.DTO.Calendar.Queries;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
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
            var sut = new GetCalendarDetailQuery.Handler(_context);

            var requestData = new IdRequest()
            {
                Id = 2
            };
            var result = await sut.Handle(new GetCalendarDetailQuery(requestData), CancellationToken.None);

            result.ShouldBeOfType<GetCalendarDetailResponse>();
            result.Id.ShouldBe(2);
            result.Events.ShouldBeOfType<List<UpdateEventRequest>>();
        }
    }
}
