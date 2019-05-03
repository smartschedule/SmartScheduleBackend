namespace SmartSchedule.Application.User.Queries.GetUserDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.User.Queries;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class GetUserDetailQuery : GetUserDetailRequest, IRequest<GetUserDetailResponse>
    {
        public class Handler : IRequestHandler<GetUserDetailQuery, GetUserDetailResponse>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<GetUserDetailResponse> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
            {
                var entity = await _context.Users.FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.User), request.Id);
                }

                return GetUserDetailResponse.Create(entity);
            }
        }
    }
}
