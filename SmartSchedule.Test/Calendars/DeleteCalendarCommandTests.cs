namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.DeleteCalendar;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class DeleteCalendarCommandTests
    {
        private readonly SmartScheduleDbContext _context;

        public DeleteCalendarCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task DeleteCalendarShouldDeleteCalendarFromDbContext()
        {
            var requestData = new DeleteCalendarRequest
            {
                CalendarId = 1
            };
            var command = new DeleteCalendarCommand(requestData);

    
            var calendar = await _context.Calendars.FindAsync(1);
            calendar.ShouldNotBeNull();

            var commandHandler = new DeleteCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var deletedCalendar = await _context.Calendars.FindAsync(1);

            deletedCalendar.ShouldBeNull();

            var UserCalendar = await _context.UserCalendars.FirstOrDefaultAsync(x => x.CalendarId == 1);

            UserCalendar.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteCalendarWithNotExistingIdShouldNotDeleteCalendarFromDbContext()
        {
            var requestData = new DeleteCalendarRequest
            {
                CalendarId = 100
            };
            var command = new DeleteCalendarCommand(requestData);
    
            var commandHandler = new DeleteCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }

    }
}
