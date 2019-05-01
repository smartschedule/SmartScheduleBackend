namespace SmartSchedule.Application.Event.Models
{
    using System;
    using System.Linq.Expressions;
    using SmartSchedule.Domain.Entities;

    public class EventDetailModel : EventCreateModel
    {
        public int Id { get; set; }

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
