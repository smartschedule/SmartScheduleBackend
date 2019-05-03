namespace SmartSchedule.Application.User.Commands.CreateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Persistence;

    public class CreateUserCommand : CreateUserRequest, IRequest
    {
        public class Handler : IRequestHandler<CreateUserCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var hash = new HashedPassword(PasswordHelper.CreateHash(request.Password));
                await new CreateUserCommandValidator(_context).ValidateAndThrowAsync(instance: request, cancellationToken: cancellationToken);
                
                var entity = new Domain.Entities.User
                {
                    Email = request.Email,
                    Name = request.UserName,
                    Password = hash.ToSaltedPassword()
                };
                _context.Users.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
