namespace SmartSchedule.Application.Event.Queries.GetEventDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.DTO.Event.Queries;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class GetEventDetailQuery : GetEventDetailRequest, IRequest<UpdateEventRequest>
    {
        public class Handler : IRequestHandler<GetEventDetailQuery, UpdateEventRequest>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<UpdateEventRequest> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
            {
                var entity = await _context.Events.FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Event), request.Id);
                }

                return UpdateEventRequest.Create(entity);
            }
        }
    }
}
