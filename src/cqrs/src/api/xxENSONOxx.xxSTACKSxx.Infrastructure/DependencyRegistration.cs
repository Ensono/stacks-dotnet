using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Extensions;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Fakes;
#if EventPublisherNone
using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;
#endif
#if EventPublisherServiceBus
using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders.Publishers;
#endif
#if EventPublisherAwsSns || EventPublisherEventHub
using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Publishers;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Extensions;
#endif
#if DynamoDb
using Amazon.DynamoDBv2;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Repositories;
#endif
#if CosmosDb || DynamoDb
using xxENSONOxx.xxSTACKSxx.Infrastructure.Repositories;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;
using xxENSONOxx.xxSTACKSxx.Infrastructure.HealthChecks;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
#endif

namespace xxENSONOxx.xxSTACKSxx.Infrastructure;

public static class DependencyRegistration
{
    static readonly ILogger log = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("DependencyRegistration");

    /// <summary>
    /// Register dynamic services that changes between environments or context(i.e: tests)
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureProductionDependencies(IConfiguration configuration, IServiceCollection services)
    {
        services.AddSecrets();

#if EventPublisherServiceBus
        services.Configure<xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration.ServiceBusConfiguration>(configuration.GetSection("ServiceBusConfiguration"));
        services.AddServiceBus();
        services.AddTransient<IApplicationEventPublisher, EventPublisher>();
#elif EventPublisherEventHub
        services.Configure<EventHubConfiguration>(configuration.GetSection("EventHubConfiguration"));
        services.AddEventHub();
#elif EventPublisherAwsSns
        services.Configure<AwsSnsConfiguration>(configuration.GetSection("AwsSnsConfiguration"));
        services.AddAwsSns(configuration);
        services.AddTransient<IApplicationEventPublisher, SnsEventPublisher>();
#elif EventPublisherNone
        services.AddTransient<IApplicationEventPublisher, DummyEventPublisher>();
#else
        services.AddTransient<Shared.Abstractions.ApplicationEvents.IApplicationEventPublisher, DummyEventPublisher>();
#endif

#if CosmosDb
        services.Configure<CosmosDbConfiguration>(configuration.GetSection("CosmosDb"));
        services.AddCosmosDB();
        services.AddTransient<IMenuRepository, CosmosDbMenuRepository>();
#elif DynamoDb
        services.Configure<DynamoDbConfiguration>(configuration.GetSection("DynamoDb"));
        services.AddDynamoDB();
        services.AddTransient<IMenuRepository, DynamoDbMenuRepository>();
#elif InMemoryDb
        services.AddTransient<IMenuRepository, InMemoryMenuRepository>();
#else
        services.AddTransient<IMenuRepository, InMemoryMenuRepository>();
#endif
#if CosmosDb
        var healthChecks = services.AddHealthChecks();
        healthChecks.AddCheck<CustomHealthCheck>("Sample"); //This is a sample health check, remove if not needed, more info: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health
        healthChecks.AddCheck<CosmosDbDocumentStorage<Menu>>("CosmosDB");
#endif
        Debug.WriteLine("ConfigureProductionDependencies");
    }
}
