namespace SmartSchedule.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using SmartSchedule.Domain.Entities.Base;
    using SmartSchedule.Domain.Enums;

    public class Event : BaseEntity<int>
    {
        public DateTime StartDate { get; set; }
        public TimeSpan Duration { get; set; }

        public TimeSpan? ReminderBefore { get; set; }

        public TimeSpan? RepeatsEvery { get; set; }
        public DateTime? RepeatsTo { get; set; }

        public EventTypes Type { get; set; }

        public string Name { get; set; }
        public string ColorHex { get; set; }

        public int CalendarId { get; set; }
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Calendar Calendar { get; set; }
        public ICollection<UserEvent> UserEvents { get; set; }
    }
}
