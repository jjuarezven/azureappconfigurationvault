using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace IAmSap.Chapter3.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var settings = config.Build();
                config.AddAzureAppConfiguration(options =>
                {
                    // 1 To refresh the application without restarting it, we configure it to watch a specific Key inside
                    // Azure App Configuration, in this case "Version" and even we can configure the cache expiration to make that refresh fast
                    options.Connect(settings["ConnectionStrings:AppConfig"])
                    .ConfigureRefresh(refresh =>
                    {
                        refresh.Register("Version", true).SetCacheExpiration(TimeSpan.FromSeconds(5));
                    })
                    // 3 In order to the application refresh changes applied on feature flags we need to add this UseFeatureFlags
                    // and update the value of the "Version" key to trigger the refresh process
                    .UseFeatureFlags();
                });
                //config.AddAzureAppConfiguration(settings["ConnectionStrings:AppConfig"]);
                //config.SetBasePath(Directory.GetCurrentDirectory());
                //config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                //config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
                //config.AddEnvironmentVariables();
            })
                .UseStartup<Startup>();
    }
}
