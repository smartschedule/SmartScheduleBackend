namespace SmartSchedule.Application.Event.Queries.GetEventList
{
    using System.Collections.Generic;
    using SmartSchedule.Application.Event.Models;

    public class EventListViewModel
    {
        public IList<EventDetailModel> Events { get; set; }
    }
}
