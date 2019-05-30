namespace SmartSchedule.Api
{
    using System;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;
    using Serilog.Exceptions;
    using Serilog.Sinks.SystemConsole.Themes;
    using SmartSchedule.Common;

    internal static class Program
    {
        private const string SERILOG_CONSOLE_OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] <{ThreadId}> ({RequestId}~{RequestPath}) {Message:lj}{NewLine}{Exception}";
        private const string SERILOG_FILE_OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] <{ProcessName} {ProcessId}:{ThreadId}> ({RequestId}~{RequestPath}) {Message:lj}{NewLine}{Exception}";

        public const string DEV_LOG_FILEPATH = "logs/dev_.log";
        public const string PRODUCTION_LOG_FILEPATH = "logs/production_.log";

        public const int LOG_FILE_SIZE_LIMIT = 10 * 1024 * 1024;

        private static void Main(string[] args)
        {
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

                loggerConfiguration.WriteTo.Async(a => a.Logger(WriteToConsole()))
                                   .WriteTo.Async(WriteToFile(DEV_LOG_FILEPATH, 1));
            }
            else
            {
                loggerConfiguration.MinimumLevel.Information()
                                   .WriteTo.Async(WriteToFile(PRODUCTION_LOG_FILEPATH, 1));
            }
#pragma warning restore CS0162 // Unreachable code detected

            Log.Logger = loggerConfiguration.CreateLogger();

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
        private static Action<LoggerConfiguration> WriteToConsole()
        {
            return b => b.WriteTo.Async(c => c.Console(outputTemplate: SERILOG_CONSOLE_OUTPUT_TEMPLATE,
                                                       theme: AnsiConsoleTheme.Literate));
        }

        private static Action<Serilog.Configuration.LoggerSinkConfiguration> WriteToFile(string filepath, int flush)
        {
            return a => a.File(filepath,
                               outputTemplate: SERILOG_FILE_OUTPUT_TEMPLATE,
                               fileSizeLimitBytes: LOG_FILE_SIZE_LIMIT,
                               rollingInterval: RollingInterval.Day,
                               rollOnFileSizeLimit: true,
                               flushToDiskInterval: TimeSpan.FromSeconds(flush),
                               retainedFileCountLimit: 64,
                               shared: true);
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseSerilog()
                   .UseUrls("http://localhost:2137", "http://localhost:2138");

    }
}
