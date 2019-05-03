namespace SmartSchedule.Application.Friends.Queries.GetPendingUserFriendRequests
{
    using MediatR;
    using SmartSchedule.Application.DTO.Friends;

    public class GetPendingUserFriendRequestsQuery : IRequest<FriendsListViewModel>
    {
        public int UserId { get; set; }
    }
}
