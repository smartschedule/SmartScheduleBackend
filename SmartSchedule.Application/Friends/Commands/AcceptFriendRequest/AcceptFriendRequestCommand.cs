namespace SmartSchedule.Application.Friends.Commands.AcceptFriendRequest
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Persistence;

    public class AcceptFriendRequestCommand : IRequest
    {

        public class Handler : IRequestHandler<AcceptFriendRequestCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
