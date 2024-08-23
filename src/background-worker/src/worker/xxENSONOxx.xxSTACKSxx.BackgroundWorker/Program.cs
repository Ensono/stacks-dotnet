using System.IO;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration.Extensions;
using xxENSONOxx.xxSTACKSxx.Shared.DependencyInjection;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker;

public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true);
            })
            .ConfigureLogging((context, builder) =>
            {
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(context.Configuration)
                    .CreateLogger();
            })
            .UseSerilog()
            .ConfigureServices((hostContext, services) =>
            {
                // Register all handlers dynamically
                var definitions = typeof(MenuCreatedEventHandler).Assembly.GetImplementationsOf(typeof(IApplicationEventHandler<>));
                foreach (var definition in definitions)
                {
                    services.AddTransient(definition.interfaceVariation, definition.implementation);
                }

                services
                    .AddLogging()
                    .AddSecrets()
                    .Configure<ServiceBusConfiguration>(hostContext.Configuration.GetSection("ServiceBusConfiguration"))
                    .AddServiceBus();
            });
}
