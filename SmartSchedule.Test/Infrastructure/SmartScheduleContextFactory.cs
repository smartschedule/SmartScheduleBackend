using System;
using Microsoft.EntityFrameworkCore;
using SmartSchedule.Persistence;
using SmartSchedule.Domain.Entities;

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

            context.Users.AddRange(new[]
            {
                new User {Id = 2, Email = "test1@test.com", Name = "test1", Password = "asdjhskdfgsdjfhlaKSDFHKASDJHFKASDJHFASD"},
                new User {Id = 3, Email = "test2@test.com", Name = "test2", Password = "asd223ads33gsdasdsdfasdasdadasdasdwasdd"}
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
