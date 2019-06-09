namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Queries.GetCalendars;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Calendar.Queries;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class GetCalendarsListQueryHandlerTests
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetCalendarsListQueryHandlerTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetCalendarsTest()
        {
            var sut = new GetCalendarsQuery.Handler(_uow, _mapper);

            var result = await sut.Handle(new GetCalendarsQuery(), CancellationToken.None);

            result.ShouldBeOfType<GetCalendarListResponse>();
        }
    }
}
