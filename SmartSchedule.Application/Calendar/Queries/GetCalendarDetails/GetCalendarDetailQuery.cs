namespace SmartSchedule.Application.Calendar.Queries.GetCalendarDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Calendar.Queries;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces.UoW;

    public class GetCalendarDetailQuery : IRequest<GetCalendarDetailResponse>
    {
        public IdRequest Data { get; set; }

        public GetCalendarDetailQuery()
        {

        }

        public GetCalendarDetailQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetCalendarDetailQuery, GetCalendarDetailResponse>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetCalendarDetailResponse> Handle(GetCalendarDetailQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var entity = await _uow.Calendars.FindAsync(data.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Calendar), data.Id);
                }

                return GetCalendarDetailResponse.Create(entity);
            }
        }
    }
}
