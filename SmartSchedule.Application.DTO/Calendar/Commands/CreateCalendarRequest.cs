namespace SmartSchedule.Application.DTO.Calendar.Commands
{
    public class CreateCalendarRequest
    {
        public string ColorHex { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}