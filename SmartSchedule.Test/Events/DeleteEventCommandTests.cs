namespace SmartSchedule.Test.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Event.Commands.DeleteEvent;
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
        public async Task DeleteEventShouldDeleteEventFromDbContext()
        {
            var requestData = new IdRequest(2);
            var command = new DeleteEventCommand(requestData);

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
            var requestData = new IdRequest(100);
            var command = new DeleteEventCommand(requestData);

            var commandHandler = new DeleteEventCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }

    }
}
