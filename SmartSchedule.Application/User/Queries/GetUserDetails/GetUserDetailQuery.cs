namespace SmartSchedule.Application.User.Queries.GetUserDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.User.Queries;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces.UoW;

    public class GetUserDetailQuery : IRequest<GetUserDetailResponse>
    {
        public IdRequest Data { get; set; }

        public GetUserDetailQuery()
        {

        }

        public GetUserDetailQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetUserDetailQuery, GetUserDetailResponse>
        {
            private readonly IUnitOfWork _context;

            public Handler(IUnitOfWork context)
            {
                _context = context;
            }

            public async Task<GetUserDetailResponse> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var entity = await _context.Users.FindAsync(data.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.User), data.Id);
                }

                return GetUserDetailResponse.Create(entity);
            }
        }
    }
}
