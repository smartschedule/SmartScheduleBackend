namespace SmartSchedule.Application.DTO.Friends.Commands
{
    public class AcceptOrRejectFriendInvitationRequest
    {
        public int RequestingUserId { get; set; }
        public int RequestedUserId { get; set; }
    }
}
