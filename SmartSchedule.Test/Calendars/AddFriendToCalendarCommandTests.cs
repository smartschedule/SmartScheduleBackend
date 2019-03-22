namespace SmartSchedule.Test.Calendars
{
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.AddFriendToCalendar;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;
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

            var command = new AddFriendToCalendarCommand
            {
                CalendarId = 2,
                UserId = 3
            };

            var commandHandler = new AddFriendToCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var userCalendar = await _context.UserCalendars.FirstOrDefaultAsync(x => x.CalendarId == command.CalendarId & x.UserId == command.UserId);

            userCalendar.ShouldNotBeNull();
            userCalendar.ShouldBeOfType<UserCalendar>();
        }

        [Fact]
        public async Task AddFriendToCalendarProvidingWrongUserIdCShouldThrowException()
        {

            var command = new AddFriendToCalendarCommand
            {
                CalendarId = 2,
                UserId = 100
            };

            var commandHandler = new AddFriendToCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();

        }

        [Fact]
        public async Task AddFriendToCalendarProvidingWrongCalendarIdCShouldThrowException()
        {

            var command = new AddFriendToCalendarCommand
            {
                CalendarId = 2000,
                UserId = 1
            };

            var commandHandler = new AddFriendToCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();

        }

        [Fact]
        public async Task AddFriendToCalendarProvidingExistingUserCalendarShouldThrowException()
        {

            var command = new AddFriendToCalendarCommand
            {
                CalendarId = 2,
                UserId = 1
            };

            var commandHandler = new AddFriendToCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();

        }

    }
}
