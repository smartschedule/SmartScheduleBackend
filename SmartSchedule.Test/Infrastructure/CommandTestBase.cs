namespace SmartSchedule.Test.Infrastructure
{
    using System;
    using SmartSchedule.Persistence;

    public class CommandTestBase : IDisposable
    {
        protected readonly SmartScheduleDbContext _context;

        public CommandTestBase()
        {
            _context = SmartScheduleContextFactory.Create();
        }

        public void Dispose()
        {
            SmartScheduleContextFactory.Destroy(_context);
        }
    }
}
