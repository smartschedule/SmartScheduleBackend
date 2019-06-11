namespace SmartSchedule.Test.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.User.Queries;
    using SmartSchedule.Application.User.Queries.GetUserDetails;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class GetUserDetailQueryHandlerTests
    {
        private readonly IUnitOfWork _uow;

        public GetUserDetailQueryHandlerTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task GetUserDetail()
        {
            var sut = new GetUserDetailsQuery.Handler(_uow);

            var result = await sut.Handle(new GetUserDetailsQuery(new IdRequest(2)), CancellationToken.None);

            result.ShouldBeOfType<GetUserDetailResponse>();
            result.Id.ShouldBe(2);
        }
    }
}
