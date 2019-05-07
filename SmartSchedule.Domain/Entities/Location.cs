namespace SmartSchedule.Domain.Entities
{
    using System.Collections.Generic;
    using SmartSchedule.Domain.Entities.Base;

    public class Location : BaseEntity<int>
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
