namespace SmartSchedule.Domain.Entities
{
    using SmartSchedule.Domain.Entities.Base;

    public class UserCalendar : BaseEntity<int>
    {
        public int UserId { get; set; }
        public int CalendarId { get; set; }

        public virtual User User { get; set; }
        public virtual Calendar Calendar { get; set; }
    }
}
