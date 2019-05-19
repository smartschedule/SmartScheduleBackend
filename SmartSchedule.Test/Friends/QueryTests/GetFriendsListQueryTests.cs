namespace SmartSchedule.Test.Friends.QueryTests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Shouldly;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Friends.Queries;
    using SmartSchedule.Application.Friends.Queries.GetFriends;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestCollection")]
    public class GetFriendsListQueryTests
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetFriendsListQueryTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetFriendsList()
        {
            var sut = new GetFriendsListQuery.Handler(_uow, _mapper);

            var result = await sut.Handle(new GetFriendsListQuery(new IdRequest(6)), CancellationToken.None);

            result.ShouldBeOfType<FriendsListResponse>();
            result.Users.Count.ShouldBe(1);
        }
    }
}
