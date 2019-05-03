namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Queries.GetCalendarList;
    using SmartSchedule.Application.DTO.Calendar.Queries;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class GetCalendarsListQueryHandlerTests
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetCalendarsListQueryHandlerTests(TestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetCalendarsTest()
        {
            var sut = new GetCalendarsListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetCalendarsListQuery(), CancellationToken.None);

            result.ShouldBeOfType<GetCalendarListResponse>();
        }
    }
}
