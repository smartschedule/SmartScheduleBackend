
namespace SmartSchedule.Domain.Entities.Base
{
    using System;

    public interface IBaseEntity<T> where T : IComparable
    {
        DateTime Created { get; set; }
        T Id { get; set; }
        DateTime Modified { get; set; }
    }
}