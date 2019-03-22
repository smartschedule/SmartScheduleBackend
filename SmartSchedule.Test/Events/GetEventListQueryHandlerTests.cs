namespace SmartSchedule.Test.Events
{
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.Event.Queries.GetEventList;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    [Collection("TestCollection")]
    public class GetEventListQueryHandlerTests
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetEventListQueryHandlerTests(TestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetEventsTest()
        {
            var sut = new GetEventListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetEventListQuery(), CancellationToken.None);

            result.ShouldBeOfType<EventListViewModel>();
        }
    }
}
