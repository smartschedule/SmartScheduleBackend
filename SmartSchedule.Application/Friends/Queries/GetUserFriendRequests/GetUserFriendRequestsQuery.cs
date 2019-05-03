namespace SmartSchedule.Application.Friends.Queries.GetUserFriendRequests
{
    using MediatR;
    using SmartSchedule.Application.DTO.Friends.Queries;

    public class GetUserFriendRequestsQuery : FriendsUserIdRequest, IRequest<FriendsListResponse>
    {

    }
}
