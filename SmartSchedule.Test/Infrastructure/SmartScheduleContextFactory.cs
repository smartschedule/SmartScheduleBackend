namespace SmartSchedule.Test.Infrastructure
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Domain.Entities;
    using SmartSchedule.Persistence;

    public static class SmartScheduleContextFactory
    {
        public static SmartScheduleDbContext Create()
        {
            var options = new DbContextOptionsBuilder<SmartScheduleDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new SmartScheduleDbContext(options);
            var saltedPassword1 = PasswordHelper.CreateHash("test1234");
            var saltedPassword2 = PasswordHelper.CreateHash("test4321");

            context.Users.AddRange(new[]
            {
                new User {Id = 2, Email = "test1@test.com", Name = "test1", Password = saltedPassword1},
                new User {Id = 3, Email = "test2@test.com", Name = "test2", Password = saltedPassword2},
                new User {Id = 4, Email = "test3@test.com", Name = "test3", Password = saltedPassword2},
                new User {Id = 5, Email = "test4@test.com", Name = "test4", Password = saltedPassword2},
                new User {Id = 6, Email = "test5@test.com", Name = "test5", Password = saltedPassword1},
                new User {Id = 7, Email = "test6@test.com", Name = "test6", Password = saltedPassword1},
                new User {Id = 8, Email = "test7@test.com", Name = "test7", Password = saltedPassword1}
            });

            context.Friends.AddRange(new[]
            {
                new Domain.Entities.Friends {FirstUserId = 3, SecoundUserId = 4, Type = Domain.Enums.FriendshipTypes.pending_first_secound},
                new Domain.Entities.Friends {FirstUserId = 4, SecoundUserId = 2, Type = Domain.Enums.FriendshipTypes.pending_first_secound},
                new Domain.Entities.Friends {FirstUserId = 2, SecoundUserId = 3, Type = Domain.Enums.FriendshipTypes.pending_secound_first},
                new Domain.Entities.Friends {FirstUserId = 5, SecoundUserId = 4, Type = Domain.Enums.FriendshipTypes.friends },
                new Domain.Entities.Friends {FirstUserId = 5, SecoundUserId = 3, Type = Domain.Enums.FriendshipTypes.friends },
                new Domain.Entities.Friends {FirstUserId = 5, SecoundUserId = 2, Type = Domain.Enums.FriendshipTypes.block_first_secound },
                new Domain.Entities.Friends {FirstUserId = 5, SecoundUserId = 6, Type = Domain.Enums.FriendshipTypes.block_scound_first },
                new Domain.Entities.Friends {FirstUserId = 6, SecoundUserId = 4, Type = Domain.Enums.FriendshipTypes.block_both },
                new Domain.Entities.Friends {FirstUserId = 6, SecoundUserId = 3, Type = Domain.Enums.FriendshipTypes.block_both },
                new Domain.Entities.Friends {FirstUserId = 6, SecoundUserId = 2, Type = Domain.Enums.FriendshipTypes.friends },
                new Domain.Entities.Friends {FirstUserId = 7, SecoundUserId = 4, Type = Domain.Enums.FriendshipTypes.pending_first_secound},
                new Domain.Entities.Friends {FirstUserId = 7, SecoundUserId = 5, Type = Domain.Enums.FriendshipTypes.pending_first_secound},
                new Domain.Entities.Friends {FirstUserId = 6, SecoundUserId = 7, Type = Domain.Enums.FriendshipTypes.pending_secound_first},
                new Domain.Entities.Friends {FirstUserId = 2, SecoundUserId = 7, Type = Domain.Enums.FriendshipTypes.pending_first_secound},
                new Domain.Entities.Friends {FirstUserId = 7, SecoundUserId = 3, Type = Domain.Enums.FriendshipTypes.pending_secound_first},
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
                new Location{Id=3,Latitude=42.423423F, Longitude=34.23424F},
                new Location{Id=5,Latitude=42.423423F, Longitude=34.23424F}
            });

            context.Events.AddRange(new[]
            {
                new Event{Id=2, StartDate=DateTime.Now, Duration = TimeSpan.Zero, ReminderBefore = TimeSpan.Zero, RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5), Type = Domain.Enums.EventTypes.standard, Name="eventsuper", ColorHex="#ffffff", LocationId = 2, CalendarId = 3},
                new Event{Id=3, StartDate=DateTime.Now, Duration = TimeSpan.Zero, ReminderBefore = TimeSpan.Zero, RepeatsEvery = TimeSpan.Zero,
                RepeatsTo = DateTime.Now.AddDays(-5), Type = Domain.Enums.EventTypes.standard, Name="eventsuper2", ColorHex="#ffffff", LocationId = 5, CalendarId = 4}
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
