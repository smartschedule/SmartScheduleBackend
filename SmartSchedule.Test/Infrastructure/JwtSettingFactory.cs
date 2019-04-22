namespace SmartSchedule.Test.Infrastructure
{
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using SmartSchedule.Application.Models;
    public static class JwtSettingFactory
    {
        public static IOptions<JwtSettings> Create()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            return Options.Create(configuration.GetSection("JwtSettings").Get<JwtSettings>());
        }
    }
}
