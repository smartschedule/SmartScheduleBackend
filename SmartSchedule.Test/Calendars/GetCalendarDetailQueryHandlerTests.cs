namespace SmartSchedule.Test.Calendars
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Queries.GetCalendarDetails;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Calendar.Queries;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class GetCalendarDetailQueryHandlerTests
    {
        private readonly IUnitOfWork _uow;

        public GetCalendarDetailQueryHandlerTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task GetCalendarDetail()
        {
            var sut = new GetCalendarDetailsQuery.Handler(_uow);

            var requestData = new IdRequest(2);
            var result = await sut.Handle(new GetCalendarDetailsQuery(requestData), CancellationToken.None);

            result.ShouldBeOfType<GetCalendarDetailResponse>();
            result.Id.ShouldBe(2);
            result.Events.ShouldBeOfType<List<UpdateEventRequest>>();
        }
    }
}
