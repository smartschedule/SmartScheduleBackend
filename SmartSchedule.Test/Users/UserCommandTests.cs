using System;
using System.Threading;
using System.Threading.Tasks;
using SmartSchedule.Application.User.Commands.CreateUser;
using SmartSchedule.Persistence;
using SmartSchedule.Test.Infrastructure;
using Xunit;
using Shouldly;
using FluentValidation;
using Microsoft.Extensions.Options;
using SmartSchedule.Application.Models;
using SmartSchedule.Application.Interfaces;
using SmartSchedule.Infrastucture.Authentication;
using Microsoft.AspNetCore.Mvc;
using SmartSchedule.Application.Exceptions;

namespace SmartSchedule.Test.Users
{
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

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task CreateUserShouldThrowExceptionAfterProvidingSameEmail()
        {
            var command = new CreateUserCommand
            {
                UserName = "Zdzichu",
                Email = "test1@test.com",
                Password = "test123"
            };

            var commandHandler = new CreateUserCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task SignInUserWithValidCredentialsShouldReturnToken()
        {
            var credentials = new EmailSignInModel
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
            var credentials = new EmailSignInModel
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
            var credentials = new EmailSignInModel
            {
                Email = "test22@test.com",
                Password = "whatever"
            };

            IJwtService jwtService = new JwtService(_context, _jwtSettings);

            await jwtService.Login(credentials).ShouldThrowAsync<NotFoundException>();
        }
    }
}
