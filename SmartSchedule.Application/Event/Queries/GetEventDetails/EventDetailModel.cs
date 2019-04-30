namespace SmartSchedule.Application.Event.Queries.GetEventDetails
{
    using System;
    using System.Linq.Expressions;
    using Domain.Entities;
    using SmartSchedule.Domain.Enums;

    public class EventDetailModel
    {
        public int Id { get; set; }
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

        public static Expression<Func<Event, EventDetailModel>> Projection
        {
            get
            {
                return evnt => new EventDetailModel
                {
                    Id = evnt.Id,
                    StartDate = evnt.StartDate,
                    Duration = evnt.Duration,
                    ReminderBefore = evnt.ReminderBefore,
                    RepeatsEvery = evnt.RepeatsEvery,
                    RepeatsTo = evnt.RepeatsTo,
                    Type = evnt.Type,
                    Name = evnt.Name,
                    CalendarId = evnt.CalendarId,
                    Longitude = evnt.Location.Longitude,
                    Latitude = evnt.Location.Latitude
                };
            }
        }

        public static EventDetailModel Create(Event evnt)
        {
            return Projection.Compile().Invoke(evnt);
        }
    }
}
