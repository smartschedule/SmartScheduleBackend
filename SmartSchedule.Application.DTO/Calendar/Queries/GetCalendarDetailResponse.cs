namespace SmartSchedule.Application.DTO.Calendar.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using SmartSchedule.Application.DTO.Event.Commands;

    public class GetCalendarDetailResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorHex { get; set; }
        public IList<UpdateEventRequest> Events { get; set; }

        public static Expression<Func<Domain.Entities.Calendar, GetCalendarDetailResponse>> Projection
        {
            get
            {
                return calendar => new GetCalendarDetailResponse
                {
                    Id = calendar.Id,
                    Name = calendar.Name,
                    ColorHex = calendar.ColorHex,
                    Events = calendar.Events.Select(y => UpdateEventRequest.Create(y)).ToList()
                };
            }
        }

        public static GetCalendarDetailResponse Create(Domain.Entities.Calendar calendar)
        {
            return Projection.Compile().Invoke(calendar);
        }
    }
}
