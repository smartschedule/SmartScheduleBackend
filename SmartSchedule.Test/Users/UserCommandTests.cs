using System;
using System.Threading;
using System.Threading.Tasks;
using SmartSchedule.Application.User.Commands.CreateUser;
using SmartSchedule.Persistence;
using SmartSchedule.Test.Infrastructure;
using Xunit;
using Shouldly;
using FluentValidation;

namespace SmartSchedule.Test.Users
{
    [Collection("QueryCollection")]
    public class UserCommandTests
    {
        private readonly SmartScheduleDbContext _context;

        public UserCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task CreateUserShouldAddUserToDbContext()
        {
            var command = new CreateUserCommand
            {
                UserName = "Zdzichu",
                Email = "janusz73@gmail.com",
                Password = "test123"
            };

            var commandHandler = new CreateUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var user = await _context.Users.FindAsync(3);
            user.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateUserShouldThrowExceptionAfterProvidingWrongData()
        {
            var command = new CreateUserCommand
            {
                UserName = "Zdzichu",
                Email = "janusz73@gmail.com",
                Password = "test"
            };

            var commandHandler = new CreateUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<ValidationException>();
        }
    }
}
