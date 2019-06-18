using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
                    //config.SetBasePath(Directory.GetCurrentDirectory());

                    //this config file will be injected in the /app/config dolder withing the container. Do not put a file in there
                    //the sdk load the appsettings.json from the project root by default
                    //this files will ovewriten the differences. In the future we might temove the root file
                    //to avoid configuration conflicts 
                    config.AddJsonFile($"config/appsettings.json", optional: true, reloadOnChange: false);
                })
                .UseStartup<Startup>();
    }
}
