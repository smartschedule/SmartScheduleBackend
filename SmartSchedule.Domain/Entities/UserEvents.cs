namespace SmartSchedule.Domain.Entities
{
    using SmartSchedule.Domain.Entities.Base;

    public class UserEvents : BaseEntity<int>
    {
        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}
