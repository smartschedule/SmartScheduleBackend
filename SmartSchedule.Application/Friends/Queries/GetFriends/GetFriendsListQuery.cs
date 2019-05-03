namespace SmartSchedule.Application.Friends.Queries.GetFriends
{
    using MediatR;
    using SmartSchedule.Application.DTO.Friends.Queries;

    public class GetFriendsListQuery : FriendsUserIdRequest, IRequest<FriendsListResponse>
    {

    }
}
