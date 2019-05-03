namespace SmartSchedule.Application.DTO.Friends.Commands
{
    public class AcceptOrRejectFriendRequestRequest
    {
        public int RequestingUserId { get; set; }
        public int RequestedUserId { get; set; }
    }
}
