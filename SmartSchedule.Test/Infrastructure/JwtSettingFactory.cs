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

            configuration.AddJsonFile(GlobalConfig.CONNECTION_STRING_NAME);

            var buildedConfiguration = configuration.AddEnvironmentVariables()
                                                    .Build();

            return Options.Create(buildedConfiguration.GetSection("JwtSettings").Get<JwtSettings>());
        }
    }
}
