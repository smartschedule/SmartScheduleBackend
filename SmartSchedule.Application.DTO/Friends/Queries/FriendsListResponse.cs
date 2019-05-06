namespace SmartSchedule.Application.DTO.Friends.Queries
{
    using System.Collections.Generic;
    using SmartSchedule.Application.DTO.User;

    public class FriendsListResponse
    {
        public List<UserLookupModel> Users { get; set; }
    }
}
