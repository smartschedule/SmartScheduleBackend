using System;
using AutoMapper;
using SmartSchedule.Persistence;
using Xunit;

namespace SmartSchedule.Test.Infrastructure
{
    public class TestFixture : IDisposable
    {
        public SmartScheduleDbContext Context { get; private set; }
        public IMapper Mapper { get; private set; }

        public TestFixture()
        {
            Context = SmartScheduleContextFactory.Create();
            Mapper = AutoMapperFactory.Create();
        }

        public void Dispose()
        {
            SmartScheduleContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestFixture> { }
}
