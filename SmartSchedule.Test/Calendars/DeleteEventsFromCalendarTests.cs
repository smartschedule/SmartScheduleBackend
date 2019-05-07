namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.DeleteEventsFromCalendar;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
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
            var requestData = new DeleteEventsFromCalendarRequest
            {
                CalendarId = 2
            };
            var command = new DeleteEventsFromCalendarCommand(requestData);
      
            var commandHandler = new DeleteEventsFromCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var calendar = await _context.Calendars.FindAsync(2);

            calendar.Events.ShouldBeEmpty();
        }

        [Fact]
        public async Task DeleteEventsFromCalendarProvidingNotExistingCalendarIdShouldThrowException()
        {
            var requestData = new DeleteEventsFromCalendarRequest
            {
                CalendarId = 200
            };
            var command = new DeleteEventsFromCalendarCommand(requestData);

            var commandHandler = new DeleteEventsFromCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>(); ;
        }

    }
}
