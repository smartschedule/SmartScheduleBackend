namespace SmartSchedule.Application.User.Commands.UpdateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Persistence;
    public class UpdateUserCommand : IRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public class Handler : IRequestHandler<UpdateUserCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context
                ;
            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(request.Id);

                if (user == null)
                {
                    throw new NotFoundException("User", request.Id);
                }

                var hash = new HashedPassword(PasswordHelper.CreateHash(request.Password));
                var vResult = await new UpdateUserCommandValidator(_context).ValidateAsync(request, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                user.Name = request.UserName;
                user.Email = request.Email;
                user.Password = hash.ToSaltedPassword();

                _context.Users.Update(user);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
