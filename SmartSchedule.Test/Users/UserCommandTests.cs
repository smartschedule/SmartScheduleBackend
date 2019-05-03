namespace SmartSchedule.Test.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Shouldly;
    using SmartSchedule.Application.DTO.Authorization;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.User.Commands.DeleteUser;
    using SmartSchedule.Application.User.Commands.UpdateUser;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class UserCommandTests
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public UserCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
            _jwtSettings = fixture.JwtSettings;
        }

        [Fact]
        public async Task DeleteUserWithValidIdShouldDeleteUser()
        {
            var command = new DeleteUserCommand
            {
                Id = 2
            };
            var deletedUser = await _context.Users.FindAsync(2);
            deletedUser.ShouldNotBeNull();

            var commandHandler = new DeleteUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            deletedUser = await _context.Users.FindAsync(2);

            deletedUser.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteUserWithInvalidIdShouldThrowNotFoundException()
        {
            var command = new DeleteUserCommand
            {
                Id = 66
            };
            var commandHandler = new DeleteUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateUserWithValidDataShouldUpdateUser()
        {
            var command = new UpdateUserCommand
            {
                Id = 3,
                UserName = "Zdzichu",
                Email = "test2@test.com",
                Password = "test123"
            };
            var commandHandler = new UpdateUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var updatedUser = await _context.Users.FindAsync(3);

            updatedUser.Name.ShouldBe(command.UserName);
            updatedUser.Password.ShouldNotBe(command.Password);
            updatedUser.Email.ShouldBe(command.Email);
            updatedUser.Id.ShouldBe(command.Id);
        }

        [Fact]
        public async Task UpdateUserWithInValidPasswordShouldThrowException()
        {
            var command = new UpdateUserCommand
            {
                Id = 3,
                UserName = "Zdzichu",
                Email = "test2@test.com",
                Password = "123"
            };
            var commandHandler = new UpdateUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task UpdateUserWithExistingEmailShouldThrowException()
        {
            var command = new UpdateUserCommand
            {
                Id = 3,
                UserName = "Zdzichu",
                Email = "test3@test.com",
                Password = "123123123"
            };
            var commandHandler = new UpdateUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
