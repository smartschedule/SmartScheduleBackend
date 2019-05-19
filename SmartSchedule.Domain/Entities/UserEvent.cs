namespace SmartSchedule.Domain.Entities
{
    using SmartSchedule.Domain.Entities.Base;

    public class UserEvent : BaseEntity<int>
    {
        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}
