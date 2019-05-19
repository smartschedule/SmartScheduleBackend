namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.DeleteCalendar;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class DeleteCalendarCommandTests
    {
        private readonly IUnitOfWork _uow;

        public DeleteCalendarCommandTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task DeleteCalendarShouldDeleteCalendarFromDbContext()
        {
            var requestData = new IdRequest(1);
            var command = new DeleteCalendarCommand(requestData);


            var calendar = await _uow.CalendarsRepository.GetByIdAsync(1);
            calendar.ShouldNotBeNull();

            var commandHandler = new DeleteCalendarCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var deletedCalendar = await _uow.CalendarsRepository.GetByIdAsync(1);

            deletedCalendar.ShouldBeNull();

            var UserCalendar = await _uow.UserCalendarsRepository.FirstOrDefaultAsync(x => x.CalendarId == 1);

            UserCalendar.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteCalendarWithNotExistingIdShouldNotDeleteCalendarFromDbContext()
        {
            var requestData = new IdRequest(100);
            var command = new DeleteCalendarCommand(requestData);

            var commandHandler = new DeleteCalendarCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>();
        }

    }
}
