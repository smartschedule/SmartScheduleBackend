namespace SmartSchedule.Domain.Entities.Base
{
    using System;

    public class BaseEntity<T>
    {
        public T Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
