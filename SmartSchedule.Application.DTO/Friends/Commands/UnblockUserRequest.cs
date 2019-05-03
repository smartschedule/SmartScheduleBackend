namespace SmartSchedule.Application.DTO.Friends.Commands
{
    public class UnblockUserRequest
    {
        public int UserId { get; set; }
        public int UserToUnblockId { get; set; }
    }
}
