namespace SmartSchedule.Application.Friends.Commands.BlockUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class BlockUserCommand : IRequest
    {

        public class Handler : IRequestHandler<BlockUserCommand, Unit>
        {
            public async Task<Unit> Handle(BlockUserCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
