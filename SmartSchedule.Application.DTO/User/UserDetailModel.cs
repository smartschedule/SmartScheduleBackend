namespace SmartSchedule.Application.DTO.User
{
    using System;
    using System.Linq.Expressions;

    public class UserDetailModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public static Expression<Func<Domain.Entities.User, UserDetailModel>> Projection
        {
            get
            {
                return user => new UserDetailModel
                {
                    Id = user.Id,
                    UserName = user.Name,
                    Email = user.Email
                };
            }
        }

        public static UserDetailModel Create(Domain.Entities.User user)
        {
            return Projection.Compile().Invoke(user);
        }
    }
}
