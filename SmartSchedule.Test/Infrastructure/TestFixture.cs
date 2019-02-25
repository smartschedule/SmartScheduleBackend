using System;
using AutoMapper;
using Microsoft.Extensions.Options;
using SmartSchedule.Application.Models;
using SmartSchedule.Persistence;
using Xunit;

namespace SmartSchedule.Test.Infrastructure
{
    public class TestFixture : IDisposable
    {
        public SmartScheduleDbContext Context { get; private set; }
        public IMapper Mapper { get; private set; }
        public IOptions<JwtSettings> JwtSettings { get; private set; }

        public TestFixture()
        {
            Context = SmartScheduleContextFactory.Create();
            Mapper = AutoMapperFactory.Create();
            JwtSettings = JwtSettingFactory.Create();
        }

        public void Dispose()
        {
            SmartScheduleContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("TestCollection")]
    public class QueryCollection : ICollectionFixture<TestFixture> { }
}
