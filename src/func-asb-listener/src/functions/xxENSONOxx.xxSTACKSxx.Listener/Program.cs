using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using xxENSONOxx.xxSTACKSxx.Listener;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(context.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
            .Build();

        services
            .Configure<StacksListener>(configuration.GetSection(nameof(StacksListener)))
            .AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddOpenTelemetry(options =>
                {
                    options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("xxENSONOxx.xxSTACKSxx.Listener"));
                    options.IncludeFormattedMessage = true;
                    options.IncludeScopes = true;
                    options.AddConsoleExporter();
                });
            });
            
        services.AddOpenTelemetry().WithTracing(builder =>
        {
            builder.AddAspNetCoreInstrumentation()                      
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter();
        });

        services.AddOpenTelemetry().WithMetrics(builder =>
        {
            builder.AddAspNetCoreInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter();
        });

        services.AddOpenTelemetry().UseOtlpExporter();

        services.AddOpenTelemetry().UseAzureMonitor();
    })
    .Build();

await host.RunAsync();
