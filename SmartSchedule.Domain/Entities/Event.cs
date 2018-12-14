using System;
using System.Collections.Generic;
using System.Text;
using SmartSchedule.Domain.Entities.Base;

namespace SmartSchedule.Domain.Entities
{
    public class Event : BaseEntity<int>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ReminderAt { get; set; }
        public string Name { get; set; }
        public int RepeatsEvery { get; set; }
        public int CalendarId { get; set; }
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Calendar Calendar { get; set; }
        public ICollection<UserEvents> UserEvents { get; set; }
    }
}
