namespace SmartSchedule.Application.Calendar.Queries.GetCalendarDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Calendar.Queries;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class GetCalendarDetailQuery : IRequest<GetCalendarDetailResponse>
    {
        public GetCalendarDetailRequest Data { get; set; }

        public GetCalendarDetailQuery()
        {

        }

        public GetCalendarDetailQuery(GetCalendarDetailRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetCalendarDetailQuery, GetCalendarDetailResponse>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<GetCalendarDetailResponse> Handle(GetCalendarDetailQuery request, CancellationToken cancellationToken)
            {
                GetCalendarDetailRequest data = request.Data;

                var entity = await _context.Calendars.FindAsync(data.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Calendar), data.Id);
                }

                return GetCalendarDetailResponse.Create(entity);
            }
        }
    }
}
