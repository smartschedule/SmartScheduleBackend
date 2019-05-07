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
                Latitude = 43.38247F,
                Longitude = 59.27492F
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
                Duration = TimeSpan.FromHours(2),
                ReminderBefore = TimeSpan.Zero,
                RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5),
                Type = Domain.Enums.EventTypes.standard,
                Name = "Event2",
                ColorHex = "#ffffff",
                Latitude = 43.38247F,
                Longitude = 59.27492F
            };

            var commandHandler = new UpdateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();

        }

        [Fact]
        public async Task UpdateEventShouldThrowExceptionAfterProvidingWrongLatitude()
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
                Latitude = 91F,
                Longitude = 59.27492F
            };

            var commandHandler = new UpdateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task UpdateEventShouldThrowExceptionAfterProvidingWrongLongitude()
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
                Latitude = 43.38247F,
                Longitude = 91F
            };

            var commandHandler = new UpdateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        //[Theory]
        //[InlineData(90F)]
        //[InlineData(-90F)]
        //public void UpdateEventShouldNotThrowExceptionAfterProvidingEdgeLatitude(float value)
        //{
        //    var command = new UpdateEventCommand
        //    {
        //        Id = 1,
        //        StartDate = DateTime.Now.AddDays(1),
        //        Duration = TimeSpan.FromHours(2),
        //        ReminderBefore = TimeSpan.Zero,
        //        RepeatsEvery = TimeSpan.Zero,
        //        RepeatsTo = DateTime.Now.AddDays(-5),
        //        Type = Domain.Enums.EventTypes.standard,
        //        Name = "Event2",
        //        ColorHex = "#ffffff",
        //        Latitude = value,
        //        Longitude = 43.38247F
        //    };

        //    var commandHandler = new UpdateEventCommand.Handler(_context);

        //    Action testCode = async () => { await commandHandler.Handle(command, CancellationToken.None); };
        //    Assert.Null(Record.Exception(testCode));
        //}

        //[Theory]
        //[InlineData(90F)]
        //[InlineData(-90F)]
        //public void UpdateEventShouldNotThrowExceptionAfterProvidingEdgeLongitude(float value)
        //{
        //    var command = new UpdateEventCommand
        //    {
        //        Id = 1,
        //        StartDate = DateTime.Now.AddDays(1),
        //        Duration = TimeSpan.FromHours(2),
        //        ReminderBefore = TimeSpan.Zero,
        //        RepeatsEvery = TimeSpan.Zero,
        //        RepeatsTo = DateTime.Now.AddDays(-5),
        //        Type = Domain.Enums.EventTypes.standard,
        //        Name = "Event2",
        //        ColorHex = "#ffffff",
        //        Latitude = 43.38247F,
        //        Longitude = value
        //    };

        //    var commandHandler = new UpdateEventCommand.Handler(_context);

        //    Action testCode = async () => { await commandHandler.Handle(command, CancellationToken.None); };
        //    Assert.Null(Record.Exception(testCode));
        //}


        [Theory]
        [InlineData("#fffffz")]
        [InlineData("ffffff")]
        [InlineData("fff")]
        [InlineData("#0123456")]
        [InlineData("#01234")]
        [InlineData("#0123")]
        [InlineData("#01")]
        [InlineData("#0")]
        public async Task UpdateEventProvidingWrongColorShouldNotUpdateEventInDbContext(string color)
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
                ColorHex = color,
                Latitude = 43.38247F,
                Longitude = 59.27492F
            };

            var commandHandler = new UpdateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

    }
}
