namespace SmartSchedule.Application.Friends.Queries.GetPendingUserFriendRequests
{
    using MediatR;
    using SmartSchedule.Application.DTO.Friends.Queries;

    public class GetPendingUserFriendRequestsQuery : FriendsUserIdRequest, IRequest<FriendsListResponse>
    {

    }
}
