namespace SmartSchedule.Application.User.Commands.DeleteUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces.UoW;

    public class DeleteUserCommand : IRequest
    {
        public IdRequest Data { get; set; }

        public DeleteUserCommand()
        {

        }

        public DeleteUserCommand(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteUserCommand, Unit>
        {
            private readonly IUnitOfWork _context;

            public Handler(IUnitOfWork context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

                if (user == null)
                {
                    throw new NotFoundException("User", data.Id);
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
