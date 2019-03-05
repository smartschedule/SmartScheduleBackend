namespace SmartSchedule.Application.User.Queries.GetUserList
{
    using System.Collections.Generic;
    using SmartSchedule.Application.Models;

    public class UserListViewModel
    {
        public IList<UserLookupModel> Users { get; set; }
    }
}
