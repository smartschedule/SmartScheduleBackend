namespace SmartSchedule.Test.Events
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Event.Commands.CreateEvent;
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
        public async Task CreateEventShouldAddCalendarToDbContext()
        {

            var command = new CreateEventCommand
            {
                StartDate = DateTime.Now,
                Duration = TimeSpan.FromHours(2),
                ReminderBefore = TimeSpan.Zero,
                RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5),
                Type = Domain.Enums.EventTypes.standard,
                Name = "Event1",
                ColorHex = "#ffffff",
                CalendarId = 2,
                Latitude = "37.38231",
                Longitude = "53.27492"
            };

            var commandHandler = new CreateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var eventE = await _context.Events.FindAsync(1);
            eventE.ShouldNotBeNull();

            eventE.Name.ShouldBe(command.Name);
            eventE.ColorHex.ShouldBe(command.ColorHex);
            eventE.StartDate.ShouldBe(command.StartDate);
            eventE.Duration.ShouldBe(command.Duration);
            eventE.ReminderBefore.ShouldBe(command.ReminderBefore);
            eventE.RepeatsEvery.ShouldBe(command.RepeatsEvery);
            eventE.RepeatsTo.ShouldBe(command.RepeatsTo);
            eventE.Type.ShouldBe(command.Type);
            eventE.RepeatsEvery.ShouldBe(command.RepeatsEvery);
            eventE.Location.Latitude.ShouldBe(command.Latitude);

            var Location = await _context.Locations.FindAsync(1);

            var calendar = await _context.Calendars.FindAsync(2);

            calendar.Events.ShouldNotBeEmpty();
            Location.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateEventShouldThrowExceptionAfterProvidingNotExistingCalendar()
        {
            var command = new CreateEventCommand
            {
                StartDate = DateTime.Now,
                Duration = TimeSpan.Zero,
                ReminderBefore = TimeSpan.Zero,
                RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5),
                Type = Domain.Enums.EventTypes.standard,
                Name = "Event1",
                ColorHex = "#ffffff",
                CalendarId = 200,
                Latitude = "37.38231",
                Longitude = "53.27492"
            };

            var commandHandler = new CreateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task CreateEventShouldThrowExceptionAfterProvidingEmptyEventName()
        {
            var command = new CreateEventCommand
            {
                StartDate = DateTime.Now,
                Duration = TimeSpan.Zero,
                ReminderBefore = TimeSpan.Zero,
                RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5),
                Type = Domain.Enums.EventTypes.standard,
                Name = "",
                ColorHex = "#ffffff",
                CalendarId = 2,
                Latitude = "37.38231",
                Longitude = "53.27492"
            };

            var commandHandler = new CreateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task CreateEventShouldThrowExceptionAfterProvidingEmptyLatitude()
        {
            var command = new CreateEventCommand
            {
                StartDate = DateTime.Now,
                Duration = TimeSpan.Zero,
                ReminderBefore = TimeSpan.Zero,
                RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5),
                Type = Domain.Enums.EventTypes.standard,
                Name = "Event1",
                ColorHex = "#ffffff",
                CalendarId = 2,
                Latitude = "",
                Longitude = "53.27492"
            };

            var commandHandler = new CreateEventCommand.Handler(_context);

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
        public async Task CreateEventShouldThrowExceptionAfterProvidingWrongColor(string color)
        {
            var command = new CreateEventCommand
            {
                StartDate = DateTime.Now,
                Duration = TimeSpan.Zero,
                ReminderBefore = TimeSpan.Zero,
                RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5),
                Type = Domain.Enums.EventTypes.standard,
                Name = "Event1",
                ColorHex = color,
                CalendarId = 2,
                Latitude = "",
                Longitude = "53.27492"
            };

            var commandHandler = new CreateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

    }
}
