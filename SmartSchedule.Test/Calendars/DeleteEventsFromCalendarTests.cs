namespace SmartSchedule.Test.Calendars
{
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.DeleteEventsFromCalendar;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    [Collection("TestCollection")]
    public class DeleteEventsFromCalendarCommandTests
    {
        private readonly SmartScheduleDbContext _context;
        public DeleteEventsFromCalendarCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task DeleteEventsFromCalendarShouldRemoveThemInDbContext()
        {

            var command = new DeleteEventsFromCalendarCommand
            {
                CalendarId = 2
            };

            var commandHandler = new DeleteEventsFromCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var calendar = await _context.Calendars.FindAsync(2);

            calendar.Events.ShouldBeEmpty();
        }

        [Fact]
        public async Task DeleteEventsFromCalendarProvidingNotExistingCalendarIdShouldThrowException()
        {

            var command = new DeleteEventsFromCalendarCommand
            {
                CalendarId = 200
            };

            var commandHandler = new DeleteEventsFromCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>(); ;

        }

    }
}
