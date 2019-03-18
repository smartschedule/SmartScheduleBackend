namespace SmartSchedule.Application.Event.Queries.GetEventList
{
    using System.Collections.Generic;

    public class EventListViewModel
    {
        public IList<EventLookupModel> Events { get; set; }
    }
}
