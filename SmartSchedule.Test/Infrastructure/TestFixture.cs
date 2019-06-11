namespace SmartSchedule.Test.Infrastructure
{
    using System;
    using AutoMapper;
    using Microsoft.Extensions.Options;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Authentication;
    using SmartSchedule.Infrastructure.UoW;
    using SmartSchedule.Persistence;
    using Xunit;

    public class TestFixture : IDisposable
    {
        public SmartScheduleDbContext Context { get; private set; }
        public IUnitOfWork UoW { get; private set; }
        public IMapper Mapper { get; private set; }
        public IOptions<JwtSettings> JwtSettings { get; private set; }

        public TestFixture()
        {
            Context = SmartScheduleContextFactory.Create();
            UoW = new UnitOfWork(Context);
            Mapper = AutoMapperFactory.Create();
            JwtSettings = JwtSettingFactory.Create();
        }

        public void Dispose()
        {
            SmartScheduleContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("TestCollection")]
    public class QueryCollection : ICollectionFixture<TestFixture>
    {

    }

    [CollectionDefinition("FriendsTestCollection")]
    public class FriendsTestCollection : ICollectionFixture<TestFixture>
    {

    }
}
