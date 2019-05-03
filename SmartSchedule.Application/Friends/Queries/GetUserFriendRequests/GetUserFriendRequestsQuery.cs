namespace SmartSchedule.Application.Friends.Queries.GetUserFriendRequests
{
    using MediatR;
    using SmartSchedule.Application.DTO.Friends;

    public class GetUserFriendRequestsQuery : IRequest<FriendsListViewModel>
    {
        public int UserId { get; set; }
    }
}
