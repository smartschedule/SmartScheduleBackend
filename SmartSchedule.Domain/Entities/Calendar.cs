namespace SmartSchedule.Domain.Entities
{
    using System.Collections.Generic;
    using SmartSchedule.Domain.Entities.Base;

    public class Calendar : BaseEntity<int>
    {
        public string Name { get; set; }
        public string ColorHex { get; set; }

        public ICollection<Event> Events { get; set; }
        public ICollection<UserCalendar> UsersCalendars { get; set; }
    }
}
