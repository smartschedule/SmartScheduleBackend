namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.CreateCalendar;
    using SmartSchedule.Application.DTO.Calendar.Commands;
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
            var requestData = new CreateCalendarRequest
            {
                Name = "kalendarz1",
                ColorHex = "#aabbcc",
                UserId = 8
            };
            var command = new CreateCalendarCommand(requestData);
    
            var commandHandler = new CreateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Name.Equals(command.Data.Name));
            var userCalendar = await _context.UserCalendars.FirstOrDefaultAsync(x => x.UserId.Equals(command.Data.UserId));

            calendar.ShouldNotBeNull();
            userCalendar.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateCalendarShouldThrowExceptionAfterProvidingNotExistingUser()
        {
            var requestData = new CreateCalendarRequest
            {
                Name = "kalendarz1",
                ColorHex = "#aabbcc",
                UserId = 193913
            };
            var command = new CreateCalendarCommand(requestData);

            var commandHandler = new CreateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task CreateCalendarShouldThrowExceptionAfterProvidingEmptyCalendarName()
        {
            var requestData = new CreateCalendarRequest
            {
                Name = "",
                ColorHex = "#aabbcc",
                UserId = 1
            };
            var command = new CreateCalendarCommand(requestData);

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
            var requestData = new CreateCalendarRequest
            {
                Name = "",
                ColorHex = color,
                UserId = 1
            };
            var command = new CreateCalendarCommand(requestData);

            var commandHandler = new CreateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
