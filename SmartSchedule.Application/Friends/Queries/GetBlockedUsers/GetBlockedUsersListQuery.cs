namespace SmartSchedule.Application.Friends.Queries.GetBlockedUsers
{
    using MediatR;
    using SmartSchedule.Application.DTO.Friends.Queries;

    public class GetBlockedUsersListQuery : FriendsUserIdRequest, IRequest<FriendsListResponse>
    {

    }
}
