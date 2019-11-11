using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using xxAMIDOxx.xxSTACKSxx.Infrastructure;

namespace xxAMIDOxx.xxSTACKSxx.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var config = CreateConfiguration().Build();
            CreateLogger(config);
            CreateWebHostBuilder(args, config).Build().Run();
        }


        private static IConfigurationBuilder CreateConfiguration()
        {
            return new ConfigurationBuilder()
                // the appsettings.json is not added to the image to avoid configuration conflicts 
                .AddJsonFile("appsettings.json", optional: true)
                // this config file will be injected at runtime in the /app/config folder of the container. 
                // Do not put any file in there
                // In the future we should set optional: false to force the user provide a config file
                .AddJsonFile($"config/appsettings.json", optional: true, reloadOnChange: false)
            ;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfigurationRoot config) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .UseSerilog()
                .ConfigureServices(DependencyRegistration.ConfigureStaticDependencies)
                .ConfigureServices(DependencyRegistration.ConfigureProductionDependencies)
            ;

        private static void CreateLogger(IConfigurationRoot config)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
        }
    }
}
