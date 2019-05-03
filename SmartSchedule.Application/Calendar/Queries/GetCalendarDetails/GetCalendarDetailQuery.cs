namespace SmartSchedule.Application.Calendar.Queries.GetCalendarDetails
{
    using MediatR;
    using SmartSchedule.Application.DTO.Calendar.Queries;

    public class GetCalendarDetailQuery : GetCalendarDetailRequest, IRequest<GetCalendarDetailResponse>
    {

    }
}
