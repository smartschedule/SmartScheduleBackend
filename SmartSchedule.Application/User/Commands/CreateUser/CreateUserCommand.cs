﻿namespace SmartSchedule.Application.User.Commands.CreateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Persistence;

    public class CreateUserCommand : IRequest
    {
        public CreateUserRequest Data { get; set; }

        public CreateUserCommand()
        {

        }

        public CreateUserCommand(CreateUserRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<CreateUserCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                CreateUserRequest data = request.Data;

                await new CreateUserCommandValidator(_context).ValidateAndThrowAsync(instance: data, cancellationToken: cancellationToken);
                
                var entity = new Domain.Entities.User
                {
                    Email = data.Email,
                    Name = data.UserName,
                    Password = PasswordHelper.CreateHash(data.Password)
                };
                _context.Users.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
