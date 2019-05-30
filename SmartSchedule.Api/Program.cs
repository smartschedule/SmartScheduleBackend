namespace SmartSchedule.Api
{
    using System;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;
    using Serilog.Events;
    using Serilog.Exceptions;
    using Serilog.Sinks.SystemConsole.Themes;
    using SmartSchedule.Common;

    internal static class Program
    {
        public const bool DEBUG = true;

        private const string SERILOG_CONSOLE_OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] <{ThreadId}> {Message:lj}{NewLine}{Exception}";
        private const string SERILOG_FILE_OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] <{ThreadId}> {Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}";

        private static void Main(string[] args)
        {
            var loggerConfiguration = new LoggerConfiguration()
                                         .Enrich.WithExceptionDetails()
                                         .Enrich.FromLogContext()
                                         .Enrich.WithThreadId();

#pragma warning disable CS0162 // Unreachable code detected
            if (GlobalConfig.DEBUG)
            {
                loggerConfiguration.MinimumLevel.Verbose()
                                   .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                                   .WriteTo.Async(a => a.Console(outputTemplate: SERILOG_CONSOLE_OUTPUT_TEMPLATE,
                                                                 theme: SystemConsoleTheme.Colored))
                                   .WriteTo.Async(a => a.File("logs/dev.log",
                                                     outputTemplate: SERILOG_FILE_OUTPUT_TEMPLATE,
                                                     fileSizeLimitBytes: 1_000_000,
                                                     rollingInterval: RollingInterval.Day,
                                                     rollOnFileSizeLimit: true,
                                                     flushToDiskInterval: TimeSpan.FromSeconds(1),
                                                     retainedFileCountLimit: 64,
                                                     shared: true));
            }
            else
            {
                loggerConfiguration.MinimumLevel.Verbose()
                                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                    .WriteTo.Async(a => a.File("logs/production.log",
                                                      outputTemplate: SERILOG_FILE_OUTPUT_TEMPLATE,
                                                      fileSizeLimitBytes: 1_000_000,
                                                      rollingInterval: RollingInterval.Day,
                                                      rollOnFileSizeLimit: true,
                                                      flushToDiskInterval: TimeSpan.FromSeconds(5),
                                                      retainedFileCountLimit: 64,
                                                      shared: true));
            }
#pragma warning restore CS0162 // Unreachable code detected



            Log.Logger = loggerConfiguration.CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseSerilog()
                   .UseUrls("http://localhost:2137", "http://localhost:2138");

    }
}
