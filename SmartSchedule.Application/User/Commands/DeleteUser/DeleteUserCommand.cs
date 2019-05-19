namespace SmartSchedule.Application.User.Commands.DeleteUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

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
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var user = await _uow.UsersRepository.GetByIdAsync(data.Id);

                if (user == null)
                {
                    throw new NotFoundException("User", data.Id);
                }

                _uow.UsersRepository.Remove(user);
                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
