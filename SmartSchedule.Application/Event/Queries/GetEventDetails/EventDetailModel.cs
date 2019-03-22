namespace SmartSchedule.Application.Event.Queries.GetEventDetails
{
    using Domain.Entities;
    using System;
    using System.Linq.Expressions;

    public class EventDetailModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ReminderAt { get; set; }
        public string Name { get; set; }
        public int RepeatsEvery { get; set; }
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
                    EndTime = evnt.EndTime,
                    ReminderAt = evnt.ReminderAt,
                    RepeatsEvery = evnt.RepeatsEvery,
                    Name = evnt.Name,
                    CalendarId = evnt.CalendarId,
                    Latitude = evnt.Location.Latitude,
                    Longitude = evnt.Location.Longitude
                };
            }
        }

        public static EventDetailModel Create(Event evnt)
        {
            return Projection.Compile().Invoke(evnt);
        }
    }
}
