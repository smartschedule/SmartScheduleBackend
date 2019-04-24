namespace SmartSchedule.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Persistence.Infrastructure;

    public class SmartScheduleDbContextFactory : DesignTimeDbContextFactoryBase<SmartScheduleDbContext>
    {
        protected override SmartScheduleDbContext CreateNewInstance(DbContextOptions<SmartScheduleDbContext> options)
        {
            return new SmartScheduleDbContext(options);
        }
    }
}
