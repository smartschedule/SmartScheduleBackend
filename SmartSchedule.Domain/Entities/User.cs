namespace SmartSchedule.Domain.Entities
{
    using System.Collections.Generic;
    using SmartSchedule.Domain.Entities.Base;

    public class User : BaseEntity<int>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public ICollection<UserEvents> UserEvents { get; set; }
        public ICollection<UserCalendar> UserCalendars { get; set; }
    }
}
