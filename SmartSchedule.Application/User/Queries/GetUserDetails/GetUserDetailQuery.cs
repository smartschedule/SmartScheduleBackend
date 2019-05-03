namespace SmartSchedule.Application.User.Queries.GetUserDetails
{
    using MediatR;
    using SmartSchedule.Application.DTO.User.Queries;

    public class GetUserDetailQuery : GetUserDetailRequest, IRequest<GetUserDetailResponse>
    {

    }
}
