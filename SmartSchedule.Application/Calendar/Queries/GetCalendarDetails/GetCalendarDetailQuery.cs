namespace SmartSchedule.Application.Calendar.Queries.GetCalendarDetails
{
    using MediatR;
    using SmartSchedule.Application.DTO.Calendar;

    public class GetCalendarDetailQuery : IRequest<CalendarDetailModel>
    {
        public int Id { get; set; }
    }
}
