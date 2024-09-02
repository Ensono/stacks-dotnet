using System.IO;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration.Extensions;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
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
                services.AddTransient<IApplicationEventHandler<CategoryCreatedEvent>, CategoryCreatedEventHandler>();
                services.AddTransient<IApplicationEventHandler<CategoryUpdatedEvent>, CategoryUpdatedEventHandler>();
                services.AddTransient<IApplicationEventHandler<CategoryDeletedEvent>, CategoryDeletedEventHandler>();
                services.AddTransient<IApplicationEventHandler<MenuCreatedEvent>, MenuCreatedEventHandler>();
                services.AddTransient<IApplicationEventHandler<MenuUpdatedEvent>, MenuUpdatedEventHandler>();
                services.AddTransient<IApplicationEventHandler<MenuDeletedEvent>, MenuDeletedEventHandler>();
                services.AddTransient<IApplicationEventHandler<MenuItemCreatedEvent>, MenuItemCreatedEventHandler>();
                services.AddTransient<IApplicationEventHandler<MenuItemUpdatedEvent>, MenuItemUpdatedEventHandler>();
                services.AddTransient<IApplicationEventHandler<MenuItemDeletedEvent>, MenuItemDeletedEventHandler>();
                
                services
                    .AddLogging()
                    .AddSecrets()
                    .Configure<ServiceBusConfiguration>(hostContext.Configuration.GetSection("ServiceBusConfiguration"))
                    .AddServiceBus();
            });
}
