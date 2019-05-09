namespace SmartSchedule.Application.Event.Queries.GetEventDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class GetEventDetailQuery : IRequest<UpdateEventRequest>
    {
        public IdRequest Data { get; set; }

        public GetEventDetailQuery()
        {

        }

        public GetEventDetailQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetEventDetailQuery, UpdateEventRequest>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<UpdateEventRequest> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var entity = await _context.Events.FindAsync(data.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Event), data.Id);
                }

                return UpdateEventRequest.Create(entity);
            }
        }
    }
}
