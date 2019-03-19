﻿namespace SmartSchedule.Test.Calendars
{
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.DeleteFriendFromCalendar;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;
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

            var command = new DeleteFriendFromCalendarCommand
            {
                CalendarId = 2,
                UserId = 3
            };

            var commandHandler = new DeleteFriendFromCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var userCalendar = await _context.UserCalendars.FirstOrDefaultAsync(x => x.CalendarId == command.CalendarId & x.UserId == command.UserId);

            var user = await _context.Users.FindAsync(1);

            var calendar = await _context.Calendars.FindAsync(2);

            calendar.ShouldNotBeNull();
            userCalendar.ShouldBeNull();
            user.ShouldNotBeNull();
        }

        [Fact]
        public async Task DeleteFriendFromCalendarProvidingNotExistingCalendarIdShouldThrowException()
        {

            var command = new DeleteFriendFromCalendarCommand
            {
                CalendarId = 200,
                UserId = 1
            };

            var commandHandler = new DeleteFriendFromCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>(); ;

        }

        [Fact]
        public async Task DeleteFriendFromCalendarProvidingNotExistingUserIdShouldThrowException()
        {

            var command = new DeleteFriendFromCalendarCommand
            {
                CalendarId = 2,
                UserId = 1000
            };

            var commandHandler = new DeleteFriendFromCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>(); ;

        }

    }
}
