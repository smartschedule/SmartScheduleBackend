namespace SmartSchedule.Test.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.DTO.User.Queries;
    using SmartSchedule.Application.User.Queries.GetUserList;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class GetCustomersListQueryHandlerTests
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomersListQueryHandlerTests(TestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetUsersTest()
        {
            var sut = new GetUsersListQuery.Handler(_context, _mapper);

            var result = await sut.Handle(new GetUsersListQuery(), CancellationToken.None);

            result.ShouldBeOfType<GetUsersListResponse>();
        }
    }
}
