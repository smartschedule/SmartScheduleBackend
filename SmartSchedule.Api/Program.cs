namespace SmartSchedule.Api
{
    using System;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;
    using Serilog.Events;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                              .MinimumLevel.Debug()
                              .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                              .Enrich.FromLogContext()
                              .WriteTo.Async(a => a.Console())
                              .WriteTo.Async(a => a.File("logs/myapp.log",
                                                         fileSizeLimitBytes: 1_000_000,
                                                         rollingInterval: RollingInterval.Day,
                                                         rollOnFileSizeLimit: true,
                                                         shared: true))
                              .CreateLogger();

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
