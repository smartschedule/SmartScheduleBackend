namespace SmartSchedule.Test.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.User.Queries;
    using SmartSchedule.Application.User.Queries.GetUserList;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class GetCustomersListQueryHandlerTests
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetCustomersListQueryHandlerTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetUsersTest()
        {
            var sut = new GetUsersListQuery.Handler(_uow, _mapper);

            var result = await sut.Handle(new GetUsersListQuery(), CancellationToken.None);

            result.ShouldBeOfType<GetUsersListResponse>();
        }
    }
}
