namespace SmartSchedule.Test.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Event;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.Event.Queries.GetEventDetails;
    using SmartSchedule.Persistence;
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
            var sut = new GetEventDetailQuery.Handler(_uow);

            var result = await sut.Handle(new GetEventDetailQuery(new IdRequest(3)), CancellationToken.None);

            result.ShouldBeOfType<UpdateEventRequest>();
            result.Id.ShouldBe(3);
        }
    }
}
