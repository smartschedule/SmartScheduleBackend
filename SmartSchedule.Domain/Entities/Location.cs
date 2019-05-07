namespace SmartSchedule.Domain.Entities
{
    using System.Collections.Generic;
    using SmartSchedule.Domain.Entities.Base;

    public class Location : BaseEntity<int>
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
