namespace SmartSchedule.Application.Calendar.Queries.GetCalendarDetails
{
    using MediatR;

    public class GetCalendarDetailQuery : IRequest<CalendarDetailModel>
    {
        public int Id { get; set; }
    }
}
