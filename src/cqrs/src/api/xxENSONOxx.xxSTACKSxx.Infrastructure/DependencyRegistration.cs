using System;
using System.Diagnostics;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Queries;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Extensions;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Fakes;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Repositories;
#if (EventPublisherAwsSns || EventPublisherEventHub)
using xxENSONOxx.xxSTACKSxx.Infrastructure.Publishers;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Extensions;
#endif
#if (CosmosDb || DynamoDb)
using Amazon.DynamoDBv2;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Repositories;
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

#if (EventPublisherServiceBus)
        services.Configure<xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration.ServiceBusConfiguration>(configuration.GetSection("ServiceBusConfiguration"));
        services.AddServiceBus();
        services.AddTransient<IApplicationEventPublisher, xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders.Publishers.EventPublisher>();
#elif (EventPublisherEventHub)
        services.Configure<EventHubConfiguration>(configuration.GetSection("EventHubConfiguration"));
        services.AddEventHub();
#elif (EventPublisherAwsSns)
        services.Configure<AwsSnsConfiguration>(configuration.GetSection("AwsSnsConfiguration"));
        services.AddAwsSns(configuration);
        services.AddTransient<IApplicationEventPublisher, EventPublisher>();
#elif (EventPublisherNone)
        services.AddTransient<IApplicationEventPublisher, DummyEventPublisher>();
#else
        services.AddTransient<IApplicationEventPublisher, DummyEventPublisher>();
#endif

#if (CosmosDb)
        services.Configure<CosmosDbConfiguration>(configuration.GetSection("CosmosDb"));
        services.AddCosmosDB();
        services.AddTransient<IMenuRepository, CosmosDbMenuRepository>();
#elif (DynamoDb)
        services.Configure<DynamoDbConfiguration>(configuration.GetSection("DynamoDb"));
        services.AddDynamoDB();
        services.AddTransient<IMenuRepository, DynamoDbMenuRepository>();
#elif (InMemoryDb)
        services.AddTransient<IMenuRepository, InMemoryMenuRepository>();
#else
        services.AddTransient<IMenuRepository, InMemoryMenuRepository>();
#endif
        var healthChecks = services.AddHealthChecks();
#if (CosmosDb)
        healthChecks.AddCheck<CustomHealthCheck>("Sample"); //This is a sample health check, remove if not needed, more info: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health
        healthChecks.AddCheck<CosmosDbDocumentStorage<Menu>>("CosmosDB");
#endif
        Debug.WriteLine("ConfigureProductionDependencies");
    }
}
