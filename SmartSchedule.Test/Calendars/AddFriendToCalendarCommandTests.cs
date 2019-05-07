namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.AddFriendToCalendar;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class AddFriendToCalendarCommandTests
    {
        private readonly SmartScheduleDbContext _context;

        public AddFriendToCalendarCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task AddFriendToCalendarShouldAddUserCalendarToDbContext()
        {
            var requestData = new AddFriendToCalendarRequest
            {
                CalendarId = 2,
                UserId = 3
            };
            var command = new AddFriendToCalendarCommand(requestData);

            var commandHandler = new AddFriendToCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var userCalendar = await _context.UserCalendars.FirstOrDefaultAsync(x => x.CalendarId == command.Data.CalendarId && x.UserId == command.Data.UserId);

            userCalendar.ShouldNotBeNull();
            userCalendar.ShouldBeOfType<UserCalendar>();
        }

        [Fact]
        public async Task AddFriendToCalendarProvidingWrongUserIdCShouldThrowException()
        {
            var requestData = new AddFriendToCalendarRequest
            {
                CalendarId = 2,
                UserId = 100
            };
            var command = new AddFriendToCalendarCommand(requestData);

            var commandHandler = new AddFriendToCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();

        }

        [Fact]
        public async Task AddFriendToCalendarProvidingWrongCalendarIdCShouldThrowException()
        {
            var requestData = new AddFriendToCalendarRequest
            {
                CalendarId = 2000,
                UserId = 1
            };
            var command = new AddFriendToCalendarCommand(requestData);

            var commandHandler = new AddFriendToCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();

        }

        [Fact]
        public async Task AddFriendToCalendarProvidingExistingUserCalendarShouldThrowException()
        {
            var requestData = new AddFriendToCalendarRequest
            {
                CalendarId = 2,
                UserId = 1
            };
            var command = new AddFriendToCalendarCommand(requestData);

            var commandHandler = new AddFriendToCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();

        }

    }
}
