namespace SmartSchedule.Application.Calendar.Queries.GetCalendarDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class CalendarDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorHex { get; set; }
        public IList<EventLookupModel> Events { get; set; }

        public static Expression<Func<Domain.Entities.Calendar, CalendarDetailModel>> Projection
        {
            get
            {
                return calendar => new CalendarDetailModel
                {
                    Id = calendar.Id,
                    Name = calendar.Name,
                    ColorHex = calendar.ColorHex,
                    Events = calendar.Events.Select(y => new EventLookupModel
                    {
                        Id = y.Id,
                        Name = y.Name,
                        CalendarId = y.CalendarId,
                        EndTime = y.EndTime,
                        StartDate = y.StartDate,
                        Latitude = y.Location.Latitude,
                        Longitude = y.Location.Longitude

                    }).ToList()
                };
            }
        }

        public static CalendarDetailModel Create(Domain.Entities.Calendar calendar)
        {
            return Projection.Compile().Invoke(calendar);
        }
    }
}
