using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace xxAMIDOxx.xxSTACKSxx.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // this config file will be injected at runtime in the /app/config folder of the container. 
                    // Do not put any file in there
                    // the appsettings.json is not added to the image to avoid configuration conflicts 
                    // In the future we should set optional: false to force the user provide a config file
                    config.AddJsonFile($"config/appsettings.json", optional: true, reloadOnChange: false);
                })
                .UseStartup<Startup>();
    }
}
