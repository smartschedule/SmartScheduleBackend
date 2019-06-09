namespace SmartSchedule.Application.User.Commands.CreateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Application.Helpers;

    public class CreateUserCommand : IRequest
    {
        public CreateUserRequest Data { get; set; }

        public CreateUserCommand(CreateUserRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<CreateUserCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                CreateUserRequest data = request.Data;

                await new CreateUserCommandValidator(_uow).ValidateAndThrowAsync(instance: data, cancellationToken: cancellationToken);

                var entity = new Domain.Entities.User
                {
                    Email = data.Email,
                    Name = data.UserName,
                    Password = PasswordHelper.CreateHash(data.Password)
                };
                _uow.UsersRepository.Add(entity);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
