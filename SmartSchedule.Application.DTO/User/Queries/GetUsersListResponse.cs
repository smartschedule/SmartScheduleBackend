namespace SmartSchedule.Application.DTO.User.Queries
{
    using System.Collections.Generic;

    public class GetUsersListResponse
    {
        public IList<UserLookupModel> Users { get; set; }
    }
}
