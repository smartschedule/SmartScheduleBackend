using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using SmartSchedule.Application.Event.Commands.CreateEvent;
using SmartSchedule.Persistence;
using SmartSchedule.Test.Infrastructure;
using Xunit;
using System;

namespace SmartSchedule.Test.Events
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
        public async Task CreateEventShouldAddCalendarToDbContext()
        {

            var command = new CreateEventCommand
            {
                StartDate = DateTime.Now,
                EndTime=DateTime.Now.AddDays(1),
                ReminderAt=DateTime.Now.AddDays(-1),
                Name = "Event1",
                RepeatsEvery = 10,
                CalendarId = 2,
                Latitude= "37.38231",
                Longitude= "53.27492"
            };

            var commandHandler = new CreateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var Event = await _context.Events.FindAsync(1);
            var Location = await _context.Locations.FindAsync(1);

            Event.ShouldNotBeNull();
            Location.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateEventShouldThrowExceptionAfterProvidingNotExistingCalendar()
        {
            var command = new CreateEventCommand
            {
                StartDate = DateTime.Now,
                EndTime = DateTime.Now.AddDays(1),
                ReminderAt = DateTime.Now.AddDays(-1),
                Name = "Event1",
                RepeatsEvery = 10,
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
                EndTime = DateTime.Now.AddDays(1),
                ReminderAt = DateTime.Now.AddDays(-1),
                Name = "",
                RepeatsEvery = 10,
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
                EndTime = DateTime.Now.AddDays(1),
                ReminderAt = DateTime.Now.AddDays(-1),
                Name = "Event1",
                RepeatsEvery = 10,
                CalendarId = 2,
                Latitude = "",
                Longitude = "53.27492"
            };

            var commandHandler = new CreateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

    }
}
