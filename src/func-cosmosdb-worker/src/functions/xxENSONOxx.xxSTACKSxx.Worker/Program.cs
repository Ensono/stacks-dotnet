using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace xxENSONOxx.xxSTACKSxx.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder
                    .SetBasePath(context.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddEnvironmentVariables();
            })
            .ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddOpenTelemetry(options =>
                {
                    options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("xxENSONOxx.xxSTACKSxx.Listener"));
                    options.IncludeFormattedMessage = true;
                    options.IncludeScopes = true;
                    options.AddConsoleExporter();
                });
            })
            .ConfigureServices((context, services) =>
            {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();

                services.AddOpenTelemetry()
                        .UseAzureMonitor()
                        .UseFunctionsWorkerDefaults()
                        .WithTracing(builder =>
                        {
                            builder.AddConsoleExporter();
                        })
                        .WithMetrics(builder =>
                        {
                            builder.AddConsoleExporter();
                        })
                        .UseOtlpExporter();
            });
}
