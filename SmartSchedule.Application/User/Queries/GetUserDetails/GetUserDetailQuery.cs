namespace SmartSchedule.Application.User.Queries.GetUserDetails
{
    using MediatR;

    public class GetUserDetailQuery : IRequest<UserDetailModel>
    {
        public int Id { get; set; }
    }
}
