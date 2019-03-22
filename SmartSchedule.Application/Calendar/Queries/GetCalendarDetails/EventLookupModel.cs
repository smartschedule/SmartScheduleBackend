namespace SmartSchedule.Application.Calendar.Queries.GetCalendarDetails
{
    using System;

    public class EventLookupModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public int CalendarId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
