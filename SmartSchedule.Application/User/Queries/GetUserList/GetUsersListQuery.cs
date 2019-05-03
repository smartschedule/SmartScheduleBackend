namespace SmartSchedule.Application.User.Queries.GetUserList
{
    using MediatR;
    using SmartSchedule.Application.DTO.User;

    public class GetUsersListQuery : IRequest<UserListViewModel>
    {
    }
}
