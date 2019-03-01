namespace SmartSchedule.Application.Friends.Queries.GetBlockedUsers
{
    using MediatR;
    using SmartSchedule.Application.Friends.Models;
    public class GetBlockedUsersListQuery : IRequest<FriendsListViewModel>
    {
        public int UserId { get; set; }
    }
}
