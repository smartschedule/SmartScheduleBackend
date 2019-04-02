namespace SmartSchedule.Api
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    internal static class Program
    {
        private static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            .UseUrls("http://localhost:2137", "http://localhost:2138");
    }
}
