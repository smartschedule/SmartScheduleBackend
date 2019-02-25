using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmartSchedule.Application.Models;

namespace SmartSchedule.Test.Infrastructure
{
    public class JwtSettingFactory
    {
        public static IOptions<JwtSettings> Create()
        {
            var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\.."))
                + string.Format("{0}SmartSchedule.Api", Path.DirectorySeparatorChar);
           
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")    
                .AddEnvironmentVariables()
                .Build();

            return Options.Create(configuration.GetSection("JwtSettings").Get<JwtSettings>());
        }
    }
}
