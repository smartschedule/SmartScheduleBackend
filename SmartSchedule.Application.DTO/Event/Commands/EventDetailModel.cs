namespace SmartSchedule.Application.DTO.Event.Commands
{
    using System;
    using System.Linq.Expressions;
    using SmartSchedule.Domain.Entities;

    public class UpdateEventRequest : CreateEventRequest
    {
        public int Id { get; set; }

        public static Expression<Func<Event, UpdateEventRequest>> Projection
        {
            get
            {
                return evnt => new UpdateEventRequest
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

        public static UpdateEventRequest Create(Event evnt)
        {
            return Projection.Compile().Invoke(evnt);
        }
    }
}
