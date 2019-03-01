namespace SmartSchedule.Application.Friends.Queries.GetUserFriendRequests
{
    using MediatR;
    using SmartSchedule.Application.Friends.Models;

    public class GetUserFriendRequestsQuery : IRequest<FriendsListViewModel>
    {
        public int UserId { get; set; }
    }
}
