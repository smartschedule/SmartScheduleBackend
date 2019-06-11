namespace SmartSchedule.Test.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.Event.Queries.GetEventDetails;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class GetEventDetailQueryHandlerTests
    {
        private readonly IUnitOfWork _uow;

        public GetEventDetailQueryHandlerTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task GetEventDetail()
        {
            var sut = new GetEventDetailsQuery.Handler(_uow);

            var result = await sut.Handle(new GetEventDetailsQuery(new IdRequest(3)), CancellationToken.None);

            result.ShouldBeOfType<UpdateEventRequest>();
            result.Id.ShouldBe(3);
        }
    }
}
