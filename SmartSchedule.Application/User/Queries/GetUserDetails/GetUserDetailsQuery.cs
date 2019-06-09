namespace SmartSchedule.Application.User.Queries.GetUserDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.User.Queries;
    using SmartSchedule.Application.Exceptions;

    public class GetUserDetailsQuery : IRequest<GetUserDetailResponse>
    {
        public IdRequest Data { get; set; }

        public GetUserDetailsQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetUserDetailsQuery, GetUserDetailResponse>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetUserDetailResponse> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var entity = await _uow.UsersRepository.GetByIdAsync(data.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.User), data.Id);
                }

                return GetUserDetailResponse.Create(entity);
            }
        }
    }
}
