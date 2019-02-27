namespace SmartSchedule.Domain.Entities
{
    using SmartSchedule.Domain.Entities.Base;
    using SmartSchedule.Domain.Enums;

    public class Friends
    {
        public int FirstUserId { get; set; }
        public int SecoundUserId { get; set; }
        public FriendshipTypes Type { get; set; }
        public virtual User FirstUser { get; set; }
        public virtual User SecoundUser { get; set; }
    }
}
