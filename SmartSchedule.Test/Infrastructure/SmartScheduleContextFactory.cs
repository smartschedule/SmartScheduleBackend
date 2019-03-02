﻿using System;
using Microsoft.EntityFrameworkCore;
using SmartSchedule.Application.Helpers;
using SmartSchedule.Domain.Entities;
using SmartSchedule.Persistence;

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
                new User {Id = 4, Email = "test3@test.com", Name = "test3", Password = saltedPassword2},
                new User {Id = 5, Email = "test4@test.com", Name = "test4", Password = saltedPassword2},
                new User {Id = 6, Email = "test5@test.com", Name = "test5", Password = saltedPassword1}
            });

            context.Friends.AddRange(new[]
            {
                new Domain.Entities.Friends {FirstUserId = 3, SecoundUserId = 4, Type = Domain.Enums.FriendshipTypes.pending_first_secound},
                new Domain.Entities.Friends {FirstUserId = 4, SecoundUserId = 2, Type = Domain.Enums.FriendshipTypes.pending_first_secound},
                new Domain.Entities.Friends {FirstUserId = 2, SecoundUserId = 3, Type = Domain.Enums.FriendshipTypes.pending_first_secound},
                new Domain.Entities.Friends {FirstUserId = 5, SecoundUserId = 4, Type = Domain.Enums.FriendshipTypes.friends },
                new Domain.Entities.Friends {FirstUserId = 5, SecoundUserId = 3, Type = Domain.Enums.FriendshipTypes.friends },
                new Domain.Entities.Friends {FirstUserId = 5, SecoundUserId = 2, Type = Domain.Enums.FriendshipTypes.block_first_secound },
                new Domain.Entities.Friends {FirstUserId = 6, SecoundUserId = 5, Type = Domain.Enums.FriendshipTypes.block_scound_first },
                new Domain.Entities.Friends {FirstUserId = 6, SecoundUserId = 4, Type = Domain.Enums.FriendshipTypes.block_both },
                new Domain.Entities.Friends {FirstUserId = 6, SecoundUserId = 3, Type = Domain.Enums.FriendshipTypes.block_both }
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
