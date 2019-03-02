namespace SmartSchedule.Test.Friends.QueryTests
{
    using AutoMapper;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("FriendsTestQueryCollection")]
    public class GetPendingUserFriendRequestsQueryTests
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetPendingUserFriendRequestsQueryTests(TestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }
    }
}
