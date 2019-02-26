using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shouldly;
using SmartSchedule.Application.Exceptions;
using SmartSchedule.Application.Interfaces;
using SmartSchedule.Application.Models;
using SmartSchedule.Application.Calendar.Commands.DeleteCalendar;
using SmartSchedule.Application.User.Commands.CreateUser;
using SmartSchedule.Infrastucture.Authentication;
using SmartSchedule.Persistence;
using SmartSchedule.Test.Infrastructure;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace SmartSchedule.Test.Calendars
{
    [Collection("TestCollection")]
    public class DeleteCalendarCommandTests
    {
        private readonly SmartScheduleDbContext _context;
        public DeleteCalendarCommandTests(TestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task DeleteCalendarShouldDeleteCalendarFromDbContext()
        {

            var command = new DeleteCalendarCommand
            {
                Id = 1
            };
            var calendar = await _context.Calendars.FindAsync(1);
            calendar.ShouldNotBeNull();

            var commandHandler = new DeleteCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None);

            var deletedCalendar = await _context.Calendars.FindAsync(1);

            deletedCalendar.ShouldBeNull();

            var UserCalendar = await _context.UserCalendars.FirstOrDefaultAsync(x => x.CalendarId == 1);

            UserCalendar.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteCalendarWithNotExistingIdShouldNotDeleteCalendarFromDbContext()
        {

            var command = new DeleteCalendarCommand
            {
                Id = 100
            };

            var commandHandler = new DeleteCalendarCommand.Handler(_context);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<NotFoundException>(); 
        }

    }
}
