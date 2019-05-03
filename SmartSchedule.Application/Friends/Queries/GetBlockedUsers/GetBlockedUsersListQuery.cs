namespace SmartSchedule.Application.Friends.Queries.GetBlockedUsers
{
    using MediatR;
    using SmartSchedule.Application.DTO.Friends;

    public class GetBlockedUsersListQuery : IRequest<FriendsListViewModel>
    {
        public int UserId { get; set; }
    }
}
