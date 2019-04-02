using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmartSchedule.Application.Models;

namespace SmartSchedule.Test.Infrastructure
{
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
