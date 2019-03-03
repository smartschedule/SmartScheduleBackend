using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using SmartSchedule.Application.Exceptions;
using SmartSchedule.Persistence;
using SmartSchedule.Test.Infrastructure;
using Xunit;
using SmartSchedule.Application.Event.Commands.UpdateEvent;
using System;

namespace SmartSchedule.Test.Calendars
{
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
                Id=1,
                StartDate = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(-1),
                ReminderAt = DateTime.Now.AddDays(-2),
                Name = "Event2",
                RepeatsEvery = 11,
                Latitude = "43.38247",
                Longitude = "59.27492"
            };

            var commandHandler = new UpdateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var eventE = await _context.Events.FindAsync(1);

            eventE.Name.ShouldBe(command.Name);
            eventE.StartDate.ShouldBe(command.StartDate);
            eventE.EndTime.ShouldBe(command.EndTime);
            eventE.ReminderAt.ShouldBe(command.ReminderAt);
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
                EndTime = DateTime.Now.AddDays(-1),
                ReminderAt = DateTime.Now.AddDays(-2),
                Name = "Event2",
                RepeatsEvery = 11,
                Latitude = "43.38247",
                Longitude = "59.27492"
            };

            var commandHandler = new UpdateEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();

        }

    }
}
