namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.DeleteFriendFromCalendar;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class DeleteFriendFromCalendarCommandTests
    {
        private readonly IUnitOfWork _uow;

        public DeleteFriendFromCalendarCommandTests(TestFixture fixture)
        {
            _uow = fixture.UoW;
        }

        [Fact]
        public async Task DeleteFriendFromCalendarShouldRemoveUserCalendarButNoInUsersOrCalendarsInDbContext()
        {
            var requestData = new DeleteFriendFromCalendarRequest
            {
                CalendarId = 2,
                UserId = 3
            };
            var command = new DeleteFriendFromCalendarCommand(requestData);

            var commandHandler = new DeleteFriendFromCalendarCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var userCalendar = await _uow.UserCalendarsRepository.FirstOrDefaultAsync(x => x.CalendarId == requestData.CalendarId && x.UserId == requestData.UserId);

            var user = await _uow.UsersRepository.GetByIdAsync(3);

            var calendar = await _uow.CalendarsRepository.GetByIdAsync(2);

            calendar.ShouldNotBeNull();
            userCalendar.ShouldBeNull();
            user.ShouldNotBeNull();
        }

        [Fact]
        public async Task DeleteFriendFromCalendarProvidingNotExistingCalendarIdShouldThrowException()
        {
            var requestData = new DeleteFriendFromCalendarRequest
            {
                CalendarId = 200,
                UserId = 1
            };
            var command = new DeleteFriendFromCalendarCommand(requestData);

            var commandHandler = new DeleteFriendFromCalendarCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>(); ;
        }

        [Fact]
        public async Task DeleteFriendFromCalendarProvidingNotExistingUserIdShouldThrowException()
        {
            var requestData = new DeleteFriendFromCalendarRequest
            {
                CalendarId = 2,
                UserId = 1000
            };
            var command = new DeleteFriendFromCalendarCommand(requestData);

            var commandHandler = new DeleteFriendFromCalendarCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>(); ;
        }

    }
}
