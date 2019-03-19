namespace SmartSchedule.Test.Calendars
{
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.UpdateCalendar;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;
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
                ColorHex = "dziendobrytakikolor2"
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
                ColorHex = ""
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
                ColorHex = ""
            };

            var commandHandler = new UpdateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();

        }

    }
}
