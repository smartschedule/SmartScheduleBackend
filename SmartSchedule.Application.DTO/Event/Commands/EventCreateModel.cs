namespace SmartSchedule.Application.DTO.Event.Commands
{
    using System;
    using SmartSchedule.Domain.Enums;

    public class CreateEventRequest
    {
        public DateTime StartDate { get; set; }
        public TimeSpan Duration { get; set; }

        public TimeSpan? ReminderBefore { get; set; }

        public TimeSpan? RepeatsEvery { get; set; }
        public DateTime? RepeatsTo { get; set; }

        public EventTypes Type { get; set; }

        public string Name { get; set; }
        public int CalendarId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
