namespace SmartSchedule.Test.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Event.Commands.DeleteEvent;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class DeleteEventCommandTests
    {
        private readonly SmartScheduleDbContext _context;

        public DeleteEventCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task DeleteEventShouldDeleteEventFromDbContext()
        {

            var command = new DeleteEventCommand
            {
                Id = 2
            };
            var eventE = await _context.Events.FindAsync(1);
            eventE.ShouldNotBeNull();

            var commandHandler = new DeleteEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var deletedEvent = await _context.Events.FindAsync(2);

            deletedEvent.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteEventWithNotExistingIdShouldNotEventCalendarFromDbContext()
        {

            var command = new DeleteEventCommand
            {
                Id = 100
            };

            var commandHandler = new DeleteEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }

    }
}
