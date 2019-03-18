namespace SmartSchedule.Application.Calendar.Queries.GetCalendarDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class GetCalendarDetailQueryHandler : IRequestHandler<GetCalendarDetailQuery, CalendarDetailModel>
    {
        private readonly SmartScheduleDbContext _context;

        public GetCalendarDetailQueryHandler(SmartScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<CalendarDetailModel> Handle(GetCalendarDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Calendars.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Calendar), request.Id);
            }

            var result = CalendarDetailModel.Create(entity);
            

            return result;
        }
    }
}
