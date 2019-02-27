using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using SmartSchedule.Application.Calendar.Commands.CreateCalendar;
using SmartSchedule.Persistence;
using SmartSchedule.Test.Infrastructure;
using Xunit;

namespace SmartSchedule.Test.Calendars
{
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
                ColorHex = "dziendobrytakikolor",
                UserId = 1
            };

            var commandHandler = new CreateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);
            
            var calendar = await _context.Calendars.FindAsync(1);
            var userCalendar = await _context.UserCalendars.FindAsync(1);

            calendar.ShouldNotBeNull();
            userCalendar.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateCalendarShouldThrowExceptionAfterProvidingNotExistingUser()
        {
            var command = new CreateCalendarCommand
            {
                Name = "kalendarz1",
                ColorHex = "dziendobrytakikolor",
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
                ColorHex = "dziendobrytakikolor",
                UserId = 1
            };

            var commandHandler = new CreateCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

    }
}
