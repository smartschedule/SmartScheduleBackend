namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.DeleteFriendFromCalendar;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class DeleteFriendFromCalendarCommandTests
    {
        private readonly SmartScheduleDbContext _context;

        public DeleteFriendFromCalendarCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
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

            var commandHandler = new DeleteFriendFromCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var userCalendar = await _context.UserCalendars.FirstOrDefaultAsync(x => x.CalendarId == requestData.CalendarId && x.UserId == requestData.UserId);

            var user = await _context.Users.FindAsync(1);

            var calendar = await _context.Calendars.FindAsync(2);

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

            var commandHandler = new DeleteFriendFromCalendarCommand.Handler(_context);

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

            var commandHandler = new DeleteFriendFromCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>(); ;
        }

    }
}
