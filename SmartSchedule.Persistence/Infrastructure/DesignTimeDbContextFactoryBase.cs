namespace SmartSchedule.Persistence.Infrastructure
{
    using System;
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using SmartSchedule.Common;

    public abstract class DesignTimeDbContextFactoryBase<TContext> :
         IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private const string ConnectionStringName = "SmartScheduleDatabase";
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        public TContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory() + string.Format("{0}..{0}SmartSchedule.Api", Path.DirectorySeparatorChar);

            return Create(basePath, Environment.GetEnvironmentVariable(AspNetCoreEnvironment));
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        private TContext Create(string basePath, string environmentName)
        {

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath);

#pragma warning disable CS0162 // Unreachable code detected
            if (GlobalConfig.DEBUG)
                configurationBuilder.AddJsonFile("appsettings.Development.json");
            else
                configurationBuilder.AddJsonFile("appsettings.json");
#pragma warning restore CS0162 // Unreachable code detected

            var configurationRoot = configurationBuilder.AddJsonFile($"appsettings.Local.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configurationRoot.GetConnectionString(ConnectionStringName);

            return Create(connectionString);
        }

        private TContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"Connection string '{ConnectionStringName}' is null or empty.", nameof(connectionString));
            }

            Console.WriteLine($"DesignTimeDbContextFactoryBase.Create(string): Connection string: '{connectionString}'.");

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return CreateNewInstance(optionsBuilder.Options);
        }
    }
}
