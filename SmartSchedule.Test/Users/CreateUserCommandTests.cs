namespace SmartSchedule.Test.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Shouldly;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Authentication;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Application.User.Commands.CreateUser;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class CreateUserCommandTests
    {
        private readonly IUnitOfWork _uow;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public CreateUserCommandTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
            _jwtSettings = fixture.JwtSettings;
        }

        [Fact]
        public async Task CreateUserShouldAddUserToDbContext()
        {
            var requestData = new CreateUserRequest
            {
                UserName = "Zdzichu",
                Email = "janusz73@gmail.com",
                Password = "test1234"
            };
            var command = new CreateUserCommand(requestData);

            var commandHandler = new CreateUserCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var user = await _uow.UsersRepository.GetByIdAsync(1);
            user.ShouldNotBeNull();
            user.Name.ShouldBe(requestData.UserName);
        }

        [Fact]
        public async Task CreateUserShouldThrowExceptionAfterProvidingWrongData()
        {
            var requestData = new CreateUserRequest
            {
                UserName = "Zdzichu",
                Email = "janusz73@gmail.com",
                Password = "test"
            };
            var command = new CreateUserCommand(requestData);

            var commandHandler = new CreateUserCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task CreateUserShouldThrowExceptionAfterProvidingSameEmail()
        {
            var requestData = new CreateUserRequest
            {
                UserName = "Zdzichu",
                Email = "test1@test.com",
                Password = "test123"
            };
            var command = new CreateUserCommand(requestData);

            var commandHandler = new CreateUserCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
