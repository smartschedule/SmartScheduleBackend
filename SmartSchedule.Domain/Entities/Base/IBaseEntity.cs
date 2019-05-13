using System;

namespace SmartSchedule.Domain.Entities.Base
{
    public interface IBaseEntity<T>
    {
        DateTime Created { get; set; }
        T Id { get; set; }
        DateTime Modified { get; set; }
    }
}