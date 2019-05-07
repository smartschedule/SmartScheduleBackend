namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.CreateCalendar;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class CreateCalendarCommandTests
    {
        private readonly SmartScheduleDbContext _context;

        public CreateCalendarCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task CreateCalendarShouldAddCalendarToDbContext()
        {

            var command = new CreateCalendarCommand
            {
                Name = "kalendarz1",
                ColorHex = "#aabbcc",
                UserId = 8
            };

            var commandHandler = new CreateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Name.Equals(command.Name));
            var userCalendar = await _context.UserCalendars.FirstOrDefaultAsync(x => x.UserId.Equals(command.UserId));

            calendar.ShouldNotBeNull();
            userCalendar.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateCalendarShouldThrowExceptionAfterProvidingNotExistingUser()
        {
            var command = new CreateCalendarCommand
            {
                Name = "kalendarz1",
                ColorHex = "#aabbcc",
                UserId = 193913
            };

            var commandHandler = new CreateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task CreateCalendarShouldThrowExceptionAfterProvidingEmptyCalendarName()
        {
            var command = new CreateCalendarCommand
            {
                Name = "",
                ColorHex = "#aabbcc",
                UserId = 1
            };

            var commandHandler = new CreateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Theory]
        [InlineData("#fffffz")]
        [InlineData("ffffff")]
        [InlineData("fff")]
        [InlineData("#0123456")]
        [InlineData("#01234")]
        [InlineData("#0123")]
        [InlineData("#01")]
        [InlineData("#0")]
        public async Task CreateCalendarShouldThrowExceptionAfterProvidingWrongColor(string color)
        {
            var command = new CreateCalendarCommand
            {
                Name = "",
                ColorHex = color,
                UserId = 1
            };

            var commandHandler = new CreateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
