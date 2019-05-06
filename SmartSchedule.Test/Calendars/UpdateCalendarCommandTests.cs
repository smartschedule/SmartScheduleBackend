namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.UpdateCalendar;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class UpdateCalendarCommandTests
    {
        private readonly SmartScheduleDbContext _context;

        public UpdateCalendarCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task UpdateCalendarShouldUpdateCalendarInDbContext()
        {

            var command = new UpdateCalendarCommand
            {
                Id = 2,
                Name = "kalendarz2",
                ColorHex = "#aabbcc"
            };

            var commandHandler = new UpdateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var calendar = await _context.Calendars.FindAsync(2);

            calendar.Name.ShouldBe(command.Name);
            calendar.ColorHex.ShouldBe(command.ColorHex);
        }

        [Fact]
        public async Task UpdateCalendarProvidingEmptyDataShouldNotUpdateCalendarInDbContext()
        {

            var command = new UpdateCalendarCommand
            {
                Id = 2,
                Name = "",
                ColorHex = "#aabbcc"
            };

            var commandHandler = new UpdateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();

        }

        [Fact]
        public async Task UpdateCalendarProvidingNotExistindIdShouldNotUpdateCalendarInDbContext()
        {

            var command = new UpdateCalendarCommand
            {
                Id = 20000,
                Name = "",
                ColorHex = "#aabbcc"
            };

            var commandHandler = new UpdateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();

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
        public async Task UpdateCalendarProvidingWrongColorShouldNotUpdateCalendarInDbContextProvidingWrongColor(string color)
        {
            var command = new UpdateCalendarCommand
            {
                Id = 2,
                Name = "testowanazwa",
                ColorHex = color
            };

            var commandHandler = new UpdateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
