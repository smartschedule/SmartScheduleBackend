namespace SmartSchedule.Application.DTO.Event.Queries
{
    using System.Collections.Generic;
    using SmartSchedule.Application.DTO.Event.Commands;

    public class GetEventListResponse
    {
        public IList<EventDetails> Events { get; set; }

        public class EventDetails : UpdateEventRequest
        {

        }
    }
}
