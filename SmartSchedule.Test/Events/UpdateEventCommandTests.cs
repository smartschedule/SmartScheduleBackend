namespace SmartSchedule.Test.Events
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Event.Commands.UpdateEvent;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class UpdateEventCommandTests
    {
        private readonly SmartScheduleDbContext _context;

        public UpdateEventCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task UpdateEventShouldUpdateEventInDbContext()
        {
            var command = new UpdateEventCommand
            {
                Id = 1,
                StartDate = DateTime.Now.AddDays(1),
                Duration = TimeSpan.FromHours(2),
                ReminderBefore = TimeSpan.Zero,
                RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5),
                Type = Domain.Enums.EventTypes.standard,
                Name = "Event2",
                ColorHex = "#ffffff",
                Latitude = "43.38247",
                Longitude = "59.27492"
            };

            var commandHandler = new UpdateEventCommand.Handler(_context);

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
        }


        [Fact]
        public async Task UpdateEventProvidingNotExistingIdShouldNotUpdateEventInDbContext()
        {

            var command = new UpdateEventCommand
            {
                Id = 1000,
                StartDate = DateTime.Now.AddDays(1),
                Duration = TimeSpan.Zero,
                ReminderBefore = TimeSpan.Zero,
                RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5),
                Type = Domain.Enums.EventTypes.standard,
                Name = "Event2",
                ColorHex = "#ffffff",
                Latitude = "43.38247",
                Longitude = "59.27492"
            };

            var commandHandler = new UpdateEventCommand.Handler(_context);

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
        public async Task UpdateEventShouldThrowExceptionAfterProvidingWrongColor(string color)
        {
            var command = new UpdateEventCommand
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

            var commandHandler = new UpdateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

    }
}
