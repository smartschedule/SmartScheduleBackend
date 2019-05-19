namespace SmartSchedule.Test.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Event.Queries;
    using SmartSchedule.Application.Event.Queries.GetEventList;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class GetEventListQueryHandlerTests
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetEventListQueryHandlerTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetEventsTest()
        {
            var sut = new GetEventListQuery.Handler(_uow, _mapper);

            var result = await sut.Handle(new GetEventListQuery(), CancellationToken.None);

            result.ShouldBeOfType<GetEventListResponse>();
        }
    }
}
