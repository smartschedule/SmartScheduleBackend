namespace SmartSchedule.Test
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Persistence;

    public class TestBase
    {
        public SmartScheduleDbContext GetDbContext(bool useSqlLite = false)
        {
            var builder = new DbContextOptionsBuilder<SmartScheduleDbContext>();
            if (useSqlLite)
            {
                builder.UseSqlite("DataSource=:memory:", x => { });
            }
            else
            {
                builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }

            var dbContext = new SmartScheduleDbContext(builder.Options);
            if (useSqlLite)
            {
                dbContext.Database.OpenConnection();
            }

            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}
