namespace SmartSchedule.Test.Friends.QueryTests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Friends.Queries;
    using SmartSchedule.Application.Friends.Queries.GetBlockedUsers;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class GetBlockedUsersListQueryTests
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetBlockedUsersListQueryTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetBlockedUsersList()
        {
            var sut = new GetBlockedUsersListQuery.Handler(_uow, _mapper);

            var result = await sut.Handle(new GetBlockedUsersListQuery(new IdRequest(6)), CancellationToken.None);

            result.ShouldBeOfType<FriendsListResponse>();
            result.Users.Count.ShouldBe(3);
        }
    }
}
