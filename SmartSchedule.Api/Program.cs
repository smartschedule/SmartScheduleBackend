namespace SmartSchedule.Api
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Serilog.Exceptions;
    using Serilog.Sinks.SystemConsole.Themes;
    using SmartSchedule.Common;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            LoggerSettings loggerSettigns;
            //logger configuration
            {
                var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());

#pragma warning disable CS0162 // Unreachable code detected
                if (GlobalConfig.DEV_MODE)
                    configuration.AddJsonFile("appsettings.Development.json");
                else
                    configuration.AddJsonFile("appsettings.json");
#pragma warning restore CS0162 // Unreachable code detected

                loggerSettigns = configuration.Build().GetSection("LoggerSettings").Get<LoggerSettings>();
            }

            var loggerConfiguration = new LoggerConfiguration()
                                         .Enrich.FromLogContext()
                                         .Enrich.WithExceptionDetails()
                                         .Enrich.WithProcessId()
                                         .Enrich.WithProcessName()
                                         .Enrich.WithThreadId();

#pragma warning disable CS0162 // Unreachable code detected
            if (GlobalConfig.DEV_MODE)
            {
                if (GlobalConfig.LOG_EVERYTHING_IN_DEV)
                    loggerConfiguration.MinimumLevel.Verbose();
                else
                    loggerConfiguration.MinimumLevel.Information();

                loggerConfiguration.WriteTo.Async(a => a.Logger(WriteToConsole(loggerSettigns)));
            }
            else
            {
                loggerConfiguration.MinimumLevel.Information();
            }
#pragma warning restore CS0162 // Unreachable code detected


            Log.Logger = loggerConfiguration.WriteTo.Async(WriteToFile(loggerSettigns))
                                            .CreateLogger();

            try
            {
#pragma warning disable CS0162 // Unreachable code detected
                if (GlobalConfig.DEV_MODE)
                {
                    Log.Warning("DEVelopment mode enabled!");
                    if (GlobalConfig.LOG_EVERYTHING_IN_DEV)
                        Log.Warning("Verbose logging mode enabled!");
                }
                else
                {
                    Log.Warning("PRODuction mode enabled!");
                }
#pragma warning restore CS0162 // Unreachable code detected


                Log.Information("Starting web host...");
                CreateWebHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return;
            }
            finally
            {
                Log.Information("Closing web host...");

                Log.CloseAndFlush();
            }
        }
        private static Action<LoggerConfiguration> WriteToConsole(LoggerSettings loggerSettigns)
        {
            return b => b.WriteTo.Async(c => c.Console(outputTemplate: loggerSettigns.ConsoleOutputTemplate,
                                                       theme: AnsiConsoleTheme.Literate));
        }

        private static Action<Serilog.Configuration.LoggerSinkConfiguration> WriteToFile(LoggerSettings loggerSettigns)
        {
            return a => a.File(loggerSettigns.FullPath,
                               outputTemplate: loggerSettigns.FileOutputTemplate,
                               fileSizeLimitBytes: loggerSettigns.FileSizeLimitInBytes,
                               rollingInterval: RollingInterval.Day,
                               rollOnFileSizeLimit: true,
                               flushToDiskInterval: TimeSpan.FromSeconds(loggerSettigns.FlushIntervalInSeconds),
                               retainedFileCountLimit: loggerSettigns.RetainedFileCountLimit,
                               shared: true);
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseSerilog()
                   .UseUrls("http://localhost:2137", "http://localhost:2138");

    }
}
