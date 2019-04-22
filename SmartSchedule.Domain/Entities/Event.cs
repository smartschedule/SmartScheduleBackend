namespace SmartSchedule.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using SmartSchedule.Domain.Entities.Base;

    public class Event : BaseEntity<int>
    {
        public DateTime StartDate { get; set; } // godzina i data rozpoczęcia eventu
        public TimeSpan Duration { get; set; } // czas trwania wydarzenia od rozpoczęcia

        public TimeSpan? ReminderBefore { get; set; } // ile minut/godzin/dni/... przed rozpoczęciem wydarzenia ma wyświetlić się przypomnienie? jeśli null to brak przypomnienia

        public TimeSpan? RepeatsEvery { get; set; } // co jaki okres czasu powtarza się wydarzenie? jeśli null to nie powtarza się
        public DateTime RepeatsTo { get; set; } // do kiedy powtarza się wydarzenie?

        public string Name { get; set; }
        public int CalendarId { get; set; }
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Calendar Calendar { get; set; }
        public ICollection<UserEvents> UserEvents { get; set; }
    }
}
