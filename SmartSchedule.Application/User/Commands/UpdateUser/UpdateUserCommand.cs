namespace SmartSchedule.Application.User.Commands.UpdateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Helpers;

    public class UpdateUserCommand : IRequest
    {
        public UpdateUserRequest Data { get; set; }

        public UpdateUserCommand(UpdateUserRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<UpdateUserCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                UpdateUserRequest data = request.Data;

                var user = await _uow.UsersRepository.GetByIdAsync(data.Id);

                if (user == null)
                {
                    throw new NotFoundException("User", data.Id);
                }

                var vResult = await new UpdateUserCommandValidator(_uow).ValidateAsync(data, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                user.Name = data.UserName;
                user.Email = data.Email;
                user.Password = PasswordHelper.CreateHash(data.Password);

                _uow.UsersRepository.Update(user);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
