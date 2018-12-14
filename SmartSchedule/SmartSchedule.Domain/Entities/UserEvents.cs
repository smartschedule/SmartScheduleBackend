using SmartSchedule.Domain.Entities.Base;

namespace SmartSchedule.Domain.Entities
{
    public class UserEvents : BaseEntity<int>
    {
        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}
