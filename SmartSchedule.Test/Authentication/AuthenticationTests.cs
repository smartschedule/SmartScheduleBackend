namespace SmartSchedule.Test.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Shouldly;
    using SmartSchedule.Application.DTO.Authentication;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Application.User.Commands.CreateUser;
    using SmartSchedule.Infrastucture.Authentication;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class AuthenticationTests
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public AuthenticationTests(TestFixture fixture)
        {
            _context = fixture.Context;
            _jwtSettings = fixture.JwtSettings;
        }

        [Fact]
        public async Task SignInUserWithValidCredentialsShouldReturnToken()
        {
            var credentials = new LoginRequest
            {
                Email = "test1@test.com",
                Password = "test1234"
            };

            IJwtService jwtService = new JwtService(_context, _jwtSettings);

            var result = await jwtService.Login(credentials);

            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task SignInUserWithInvalidPasswordShouldReturnUnauthorizedResult()
        {
            var credentials = new LoginRequest
            {
                Email = "test1@test.com",
                Password = "asdasfsdgsd"
            };

            IJwtService jwtService = new JwtService(_context, _jwtSettings);

            var result = await jwtService.Login(credentials);

            result.ShouldBeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task SignInUserWithNotExistingEmailShouldThrowNotFoundException()
        {
            var credentials = new LoginRequest
            {
                Email = "test22@test.com",
                Password = "whatever"
            };

            IJwtService jwtService = new JwtService(_context, _jwtSettings);

            await jwtService.Login(credentials).ShouldThrowAsync<NotFoundException>();
        }
    }
}
