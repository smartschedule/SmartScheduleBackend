namespace SmartSchedule.Test.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.User.Commands.DeleteUser;
    using SmartSchedule.Application.User.Commands.UpdateUser;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class UserCommandTests
    {
        private readonly IUnitOfWork _uow;

        public UserCommandTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task DeleteUserWithValidIdShouldDeleteUser()
        {
            var requestData = new IdRequest(2);
            var command = new DeleteUserCommand(requestData);

            var deletedUser = await _uow.UsersRepository.GetByIdAsync(2);
            deletedUser.ShouldNotBeNull();

            var commandHandler = new DeleteUserCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            deletedUser = await _uow.UsersRepository.GetByIdAsync(2);

            deletedUser.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteUserWithInvalidIdShouldThrowNotFoundException()
        {
            var requestData = new IdRequest(2000);
            var command = new DeleteUserCommand(requestData);

            var commandHandler = new DeleteUserCommand.Handler(_uow);

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
                Password = "test1234"
            };
            var command = new UpdateUserCommand(requestData);

            var commandHandler = new UpdateUserCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var updatedUser = await _uow.UsersRepository.GetByIdAsync(3);

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

            var commandHandler = new UpdateUserCommand.Handler(_uow);

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

            var commandHandler = new UpdateUserCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
