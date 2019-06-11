namespace SmartSchedule.Test.Events
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.Event.Commands.CreateEvent;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class CreateEventCommandTests
    {
        private readonly IUnitOfWork _uow;

        public CreateEventCommandTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task CreateEventShouldAddCalendarToDbContext()
        {
            var requestData = new CreateEventRequest
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
                Latitude = 37.38231F,
                Longitude = 53.27492F
            };
            var command = new CreateEventCommand(requestData);

            var commandHandler = new CreateEventCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var eventE = await _uow.EventsRepository.GetByIdAsync(1);
            eventE.ShouldNotBeNull();

            eventE.Name.ShouldBe(requestData.Name);
            eventE.ColorHex.ShouldBe(requestData.ColorHex);
            eventE.StartDate.ShouldBe(requestData.StartDate);
            eventE.Duration.ShouldBe(requestData.Duration);
            eventE.ReminderBefore.ShouldBe(requestData.ReminderBefore);
            eventE.RepeatsEvery.ShouldBe(requestData.RepeatsEvery);
            eventE.RepeatsTo.ShouldBe(requestData.RepeatsTo);
            eventE.Type.ShouldBe(requestData.Type);
            eventE.RepeatsEvery.ShouldBe(requestData.RepeatsEvery);
            eventE.Location.Latitude.ShouldBe(requestData.Latitude);

            var Location = await _uow.LocationsRepository.GetByIdAsync(1);

            var calendar = await _uow.CalendarsRepository.GetByIdAsync(2);

            calendar.Events.ShouldNotBeEmpty();
            Location.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateEventShouldThrowExceptionAfterProvidingNotExistingCalendar()
        {
            var requestData = new CreateEventRequest
            {
                StartDate = DateTime.Now,
                Duration = TimeSpan.FromHours(2),
                ReminderBefore = TimeSpan.Zero,
                RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5),
                Type = Domain.Enums.EventTypes.standard,
                Name = "Event1",
                ColorHex = "#ffffff",
                CalendarId = 200,
                Latitude = 37.38231F,
                Longitude = 53.27492F
            };
            var command = new CreateEventCommand(requestData);
            var commandHandler = new CreateEventCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task CreateEventShouldThrowExceptionAfterProvidingEmptyEventName()
        {
            var requestData = new CreateEventRequest
            {
                StartDate = DateTime.Now,
                Duration = TimeSpan.FromHours(2),
                ReminderBefore = TimeSpan.Zero,
                RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5),
                Type = Domain.Enums.EventTypes.standard,
                Name = "",
                ColorHex = "#ffffff",
                CalendarId = 2,
                Latitude = 37.38231F,
                Longitude = 53.27492F
            };
            var command = new CreateEventCommand(requestData);
            var commandHandler = new CreateEventCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task CreateEventShouldThrowExceptionAfterProvidingWrongLatitude()
        {
            var requestData = new CreateEventRequest
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
                Latitude = 91F,
                Longitude = 53.27492F
            };

            var command = new CreateEventCommand(requestData);
            var commandHandler = new CreateEventCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task CreateEventShouldThrowExceptionAfterProvidingWrongLongitude()
        {
            var requestData = new CreateEventRequest
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
                Latitude = 53.27492F,
                Longitude = 91F
            };

            var command = new CreateEventCommand(requestData);
            var commandHandler = new CreateEventCommand.Handler(_uow);

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
            var requestData = new CreateEventRequest
            {
                StartDate = DateTime.Now,
                Duration = TimeSpan.FromHours(2),
                ReminderBefore = TimeSpan.Zero,
                RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5),
                Type = Domain.Enums.EventTypes.standard,
                Name = "Event1",
                ColorHex = color,
                CalendarId = 2,
                Latitude = 53.27492F,
                Longitude = 53.27492F
            };
            var command = new CreateEventCommand(requestData);
            var commandHandler = new CreateEventCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

    }
}
