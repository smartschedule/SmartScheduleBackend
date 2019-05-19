namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.DeleteEventsFromCalendar;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class DeleteEventsFromCalendarCommandTests
    {
        private readonly IUnitOfWork _uow;

        public DeleteEventsFromCalendarCommandTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task DeleteEventsFromCalendarShouldRemoveThemInDbContext()
        {
            var requestData = new IdRequest(2);
            var command = new DeleteEventsFromCalendarCommand(requestData);

            var commandHandler = new DeleteEventsFromCalendarCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var calendar = await _uow.CalendarsRepository.GetByIdAsync(2);

            calendar.Events.ShouldBeEmpty();
        }

        [Fact]
        public async Task DeleteEventsFromCalendarProvidingNotExistingCalendarIdShouldThrowException()
        {
            var requestData = new IdRequest(200);
            var command = new DeleteEventsFromCalendarCommand(requestData);

            var commandHandler = new DeleteEventsFromCalendarCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>(); ;
        }

    }
}
