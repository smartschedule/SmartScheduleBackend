namespace SmartSchedule.Application.DTO.User.Queries
{
    using System;
    using System.Linq.Expressions;

    public class GetUserDetailResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public static Expression<Func<Domain.Entities.User, GetUserDetailResponse>> Projection
        {
            get
            {
                return user => new GetUserDetailResponse
                {
                    Id = user.Id,
                    UserName = user.Name,
                    Email = user.Email
                };
            }
        }

        public static GetUserDetailResponse Create(Domain.Entities.User user)
        {
            return Projection.Compile().Invoke(user);
        }
    }
}
