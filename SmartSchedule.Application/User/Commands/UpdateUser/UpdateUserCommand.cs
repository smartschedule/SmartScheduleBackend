namespace SmartSchedule.Application.User.Commands.UpdateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Persistence;

    public class UpdateUserCommand : IRequest
    {
        public UpdateUserRequest Data { get; set; }

        public UpdateUserCommand()
        {

        }

        public UpdateUserCommand(UpdateUserRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<UpdateUserCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                UpdateUserRequest data = request.Data;

                var user = await _context.Users.FindAsync(data.Id);

                if (user == null)
                {
                    throw new NotFoundException("User", data.Id);
                }

                var vResult = await new UpdateUserCommandValidator(_context).ValidateAsync(data, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                user.Name = data.UserName;
                user.Email = data.Email;
                user.Password = PasswordHelper.CreateHash(data.Password);

                _context.Users.Update(user);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
