using System;
using Microsoft.EntityFrameworkCore;
using SmartSchedule.Persistence;

namespace SmartSchedule.Test
{
    public class TestBase
    {
        public SmartScheduleDbContext GetDbContext()
        {
            var builder = new DbContextOptionsBuilder<SmartScheduleDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var dbContext = new SmartScheduleDbContext(builder.Options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}
