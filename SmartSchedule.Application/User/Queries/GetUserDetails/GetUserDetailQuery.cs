namespace SmartSchedule.Application.User.Queries.GetUserDetails
{
    using MediatR;
    using SmartSchedule.Application.DTO.User;

    public class GetUserDetailQuery : IRequest<UserDetailModel>
    {
        public int Id { get; set; }
    }
}
