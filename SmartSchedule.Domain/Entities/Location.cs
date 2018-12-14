using System.Collections.Generic;
using SmartSchedule.Domain.Entities.Base;

namespace SmartSchedule.Domain.Entities
{
    public class Location : BaseEntity<int>
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
