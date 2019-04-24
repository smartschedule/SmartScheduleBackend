namespace SmartSchedule.Application.User.Queries.GetUserDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, UserDetailModel>
    {
        private readonly SmartScheduleDbContext _context;

        public GetUserDetailQueryHandler(SmartScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<UserDetailModel> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.User), request.Id);
            }

            return UserDetailModel.Create(entity);
        }
    }
}
