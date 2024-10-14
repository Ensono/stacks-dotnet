using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Secrets;

var host = new HostBuilder()
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureLogging((context, logging) =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.AddConsoleExporter();
        });
    })
    .ConfigureServices((context, services) =>
    {
        services.AddOpenTelemetry().WithTracing(builder =>
        {
            builder.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter();
        })
        .WithMetrics(builder =>
        {
            builder.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddConsoleExporter();
        })
        .UseOtlpExporter();

        services.AddOpenTelemetry().UseAzureMonitor();

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
            .AddSingleton<ISecretResolver<string>, SecretResolver>()
            .Configure<ServiceBusConfiguration>(context.Configuration.GetSection("ServiceBusConfiguration"))
            .AddServiceBus();
    })
    .Build();

await host.RunAsync();
