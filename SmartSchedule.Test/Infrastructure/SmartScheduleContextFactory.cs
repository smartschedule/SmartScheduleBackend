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

            context.Friends.AddRange(new[]
            {
                new Domain.Entities.Friends {FirstUserId = 3, SecoundUserId = 4, Type = Domain.Enums.FriendshipTypes.pending_first_secound}
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
