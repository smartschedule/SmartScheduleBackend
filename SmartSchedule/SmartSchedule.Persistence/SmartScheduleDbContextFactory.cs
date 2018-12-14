using Microsoft.EntityFrameworkCore;
using SmartSchedule.Persistence.Infrastructure;

namespace SmartSchedule.Persistence
{
    public class SmartScheduleDbContextFactory : DesignTimeDbContextFactoryBase<SmartScheduleDbContext>
    {
        protected override SmartScheduleDbContext CreateNewInstance(DbContextOptions<SmartScheduleDbContext> options)
        {
            return new SmartScheduleDbContext(options);
        }
    }
}
