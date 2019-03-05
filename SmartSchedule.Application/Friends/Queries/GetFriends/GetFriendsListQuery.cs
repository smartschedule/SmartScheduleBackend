namespace SmartSchedule.Application.Friends.Queries.GetFriends
{
    using MediatR;
    using SmartSchedule.Application.Friends.Models;

    public class GetFriendsListQuery : IRequest<FriendsListViewModel>
    {
        public int UserId { get; set; }
    }
}
