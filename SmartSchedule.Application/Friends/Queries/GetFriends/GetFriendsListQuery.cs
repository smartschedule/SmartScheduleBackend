namespace SmartSchedule.Application.Friends.Queries.GetFriends
{
    using MediatR;
    using SmartSchedule.Application.DTO.Friends;

    public class GetFriendsListQuery : IRequest<FriendsListViewModel>
    {
        public int UserId { get; set; }
    }
}
