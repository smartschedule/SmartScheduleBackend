namespace SmartSchedule.Test.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Event.Commands.DeleteEvent;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class DeleteEventCommandTests
    {
        private readonly IUnitOfWork _uow;

        public DeleteEventCommandTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task DeleteEventShouldDeleteEventFromDbContext()
        {
            var requestData = new IdRequest(2);
            var command = new DeleteEventCommand(requestData);

            var eventE = await _uow.EventsRepository.GetByIdAsync(1);
            eventE.ShouldNotBeNull();

            var commandHandler = new DeleteEventCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var deletedEvent = await _uow.EventsRepository.GetByIdAsync(2);

            deletedEvent.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteEventWithNotExistingIdShouldNotEventCalendarFromDbContext()
        {
            var requestData = new IdRequest(100);
            var command = new DeleteEventCommand(requestData);

            var commandHandler = new DeleteEventCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }

    }
}
