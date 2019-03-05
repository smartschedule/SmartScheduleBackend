namespace SmartSchedule.Application.Friends.Queries.GetPendingUserFriendRequests
{
    using MediatR;
    using SmartSchedule.Application.Friends.Models;

    public class GetPendingUserFriendRequestsQuery : IRequest<FriendsListViewModel>
    {
        public int UserId { get; set; }
    }
}
