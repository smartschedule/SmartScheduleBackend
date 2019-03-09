using System;
using Microsoft.EntityFrameworkCore;
using SmartSchedule.Persistence;
using SmartSchedule.Domain.Entities;
using SmartSchedule.Application.Helpers;

namespace SmartSchedule.Test.Infrastructure
{
    public class SmartScheduleContextFactory
    {
        public static SmartScheduleDbContext Create()
        {
            var options = new DbContextOptionsBuilder<SmartScheduleDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new SmartScheduleDbContext(options);
            var saltedPassword1 = new HashedPassword(PasswordHelper.CreateHash("test1234")).ToSaltedPassword();
            var saltedPassword2 = new HashedPassword(PasswordHelper.CreateHash("test4321")).ToSaltedPassword();

            context.Users.AddRange(new[]
            {
                new User {Id = 2, Email = "test1@test.com", Name = "test1", Password = saltedPassword1},
                new User {Id = 3, Email = "test2@test.com", Name = "test2", Password = saltedPassword2},
                new User {Id = 4, Email = "test3@test.com", Name = "test3", Password = saltedPassword2}
            });

            context.Calendars.AddRange(new[]
            {
                new Calendar{Id=2,Name="kalendarz",ColorHex="kolor"},
                new Calendar{Id=4,Name="kalendarz",ColorHex="kolor"}
            });

            context.UserCalendars.AddRange(new[]
            {
                new UserCalendar{CalendarId=2,UserId=1},
                new UserCalendar{CalendarId=4,UserId=1}
            });

            context.Locations.AddRange(new[]
            {
                new Location{Id=3,Latitude="42.423423", Longitude="34.23424"},
                new Location{Id=5,Latitude="42.423423", Longitude="34.23424"}
            });

            context.Events.AddRange(new[]
            {
                new Event{Id=2, StartDate=DateTime.Now, EndTime=DateTime.Now.AddDays(+1), ReminderAt = DateTime.Now.AddDays(-1),
                Name="eventsuper", LocationId = 2, CalendarId = 3, RepeatsEvery = 2},
                new Event{Id=3, StartDate=DateTime.Now, EndTime=DateTime.Now.AddDays(+1), ReminderAt = DateTime.Now.AddDays(-1),
                Name="eventsuper2", LocationId = 5, CalendarId = 4, RepeatsEvery = 2}
            });

            context.SaveChanges();

            return context;
        }

        public static void Destroy(SmartScheduleDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
