namespace SmartSchedule.Application.Event.Queries.GetEventList
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Persistence;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetEventListQueryHandler : IRequestHandler<GetEventListQuery, EventListViewModel>
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IMapper _mapper;

        public GetEventListQueryHandler(SmartScheduleDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EventListViewModel> Handle(GetEventListQuery request, CancellationToken cancellationToken)
        {
            return new EventListViewModel
            {
                Events = await _context.Events.ProjectTo<EventLookupModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken)
            };
        }
    }
}
