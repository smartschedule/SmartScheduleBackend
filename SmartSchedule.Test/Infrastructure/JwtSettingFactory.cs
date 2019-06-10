namespace SmartSchedule.Test.Infrastructure
{
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using SmartSchedule.Application.DTO.Authentication;
    using SmartSchedule.Common;

    public static class JwtSettingFactory
    {
        public static IOptions<JwtSettings> Create()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

#pragma warning disable CS0162 // Unreachable code detected
            if (GlobalConfig.DEV_MODE)
                configuration.AddJsonFile("appsettings.Development.json");
            else
                configuration.AddJsonFile("appsettings.json");
#pragma warning restore CS0162 // Unreachable code detected

            var buildedConfiguration = configuration.AddEnvironmentVariables()
                                                    .Build();

            return Options.Create(buildedConfiguration.GetSection("JwtSettings").Get<JwtSettings>());
        }
    }
}
