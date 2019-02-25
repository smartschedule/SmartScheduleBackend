namespace SmartSchedule.Application.User.Commands.DeleteUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class DeleteUserCommand : IRequest
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<DeleteUserCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;
            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (user == null)
                {
                    throw new NotFoundException("User", request.Id);
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
