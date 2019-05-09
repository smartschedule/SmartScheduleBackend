namespace SmartSchedule.Test.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Shouldly;
    using SmartSchedule.Application.DTO.Authentication;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.User.Commands;
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
            var requestData = new IdRequest(2);
            var command = new DeleteUserCommand(requestData);

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
            var requestData = new IdRequest(2000);
            var command = new DeleteUserCommand(requestData);

            var commandHandler = new DeleteUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateUserWithValidDataShouldUpdateUser()
        {
            var requestData = new UpdateUserRequest
            {
                Id = 3,
                UserName = "Zdzichu",
                Email = "test2@test.com",
                Password = "test123"
            };
            var command = new UpdateUserCommand(requestData);

            var commandHandler = new UpdateUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var updatedUser = await _context.Users.FindAsync(3);

            updatedUser.Name.ShouldBe(requestData.UserName);
            updatedUser.Password.ShouldNotBe(requestData.Password);
            updatedUser.Email.ShouldBe(requestData.Email);
            updatedUser.Id.ShouldBe(requestData.Id);
        }

        [Fact]
        public async Task UpdateUserWithInValidPasswordShouldThrowException()
        {
            var requestData = new UpdateUserRequest
            {
                Id = 3,
                UserName = "Zdzichu",
                Email = "test2@test.com",
                Password = "123"
            };
            var command = new UpdateUserCommand(requestData);
   
            var commandHandler = new UpdateUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task UpdateUserWithExistingEmailShouldThrowException()
        {
            var requestData = new UpdateUserRequest
            {
                Id = 3,
                UserName = "Zdzichu",
                Email = "test3@test.com",
                Password = "123123123"
            };
            var command = new UpdateUserCommand(requestData);

            var commandHandler = new UpdateUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
