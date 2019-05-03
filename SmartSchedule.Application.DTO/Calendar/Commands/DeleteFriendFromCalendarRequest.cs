namespace SmartSchedule.Application.DTO.Calendar.Commands
{
    public class DeleteFriendFromCalendarRequest
    {
        public int CalendarId { get; set; }
        public int UserId { get; set; }
    }
}