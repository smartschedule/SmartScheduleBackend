namespace SmartSchedule.Application.DTO.Calendar.Commands
{
    public class AddFriendToCalendarRequest
    {
        public int CalendarId { get; set; }
        public int UserId { get; set; }
    }
}
