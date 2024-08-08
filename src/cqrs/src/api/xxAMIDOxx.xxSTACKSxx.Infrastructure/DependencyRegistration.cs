using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Application.CQRS.Queries;
using Amido.Stacks.Configuration.Extensions;
using Amido.Stacks.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Application.QueryHandlers;
using xxAMIDOxx.xxSTACKSxx.Domain;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Fakes;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.HealthChecks;
#if (EventPublisherAwsSns)
using Amido.Stacks.Messaging.AWS.SNS;
using Amido.Stacks.Messaging.AWS.SNS.Extensions;
#endif
#if (CosmosDb || DynamoDb)
using Amido.Stacks.DynamoDB;
using Amido.Stacks.DynamoDB.Extensions;
using Amazon.DynamoDBv2;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;
using Amido.Stacks.Data.Documents.CosmosDB.Extensions;
#endif

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure;

public static class DependencyRegistration
{
    static readonly ILogger log = Log.Logger;

    /// <summary>
    /// Register static services that does not change between environment or contexts(i.e: tests)
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureStaticDependencies(IServiceCollection services)
    {
        AddCommandHandlers(services);
        AddQueryHandlers(services);
    }

    /// <summary>
    /// Register dynamic services that changes between environments or context(i.e: tests)
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureProductionDependencies(WebHostBuilderContext context, IServiceCollection services)
    {
        services.AddSecrets();

#if (EventPublisherServiceBus)
        services.Configure<Amido.Stacks.Messaging.Azure.ServiceBus.Configuration.ServiceBusConfiguration>(context.Configuration.GetSection("ServiceBusConfiguration"));
        services.AddServiceBus();
        services.AddTransient<IApplicationEventPublisher, Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Publishers.EventPublisher>();
#elif (EventPublisherEventHub)
        services.Configure<Amido.Stacks.Messaging.Azure.EventHub.Configuration.EventHubConfiguration>(context.Configuration.GetSection("EventHubConfiguration"));
        services.AddEventHub();
        services.AddTransient<IApplicationEventPublisher, Amido.Stacks.Messaging.Azure.EventHub.Publisher.EventPublisher>();
#elif (EventPublisherAwsSns)
        services.Configure<AwsSnsConfiguration>(context.Configuration.GetSection("AwsSnsConfiguration"));
        services.AddAwsSns(context.Configuration);
        services.AddTransient<IApplicationEventPublisher, Amido.Stacks.Messaging.AWS.SNS.Publisher.EventPublisher>();
#elif (EventPublisherNone)
        services.AddTransient<IApplicationEventPublisher, DummyEventPublisher>();
#else
        services.AddTransient<IApplicationEventPublisher, DummyEventPublisher>();
#endif

#if (CosmosDb)
        services.Configure<Amido.Stacks.Data.Documents.CosmosDB.CosmosDbConfiguration>(context.Configuration.GetSection("CosmosDb"));
        services.AddCosmosDB();
        services.AddTransient<IMenuRepository, CosmosDbMenuRepository>();
#elif (DynamoDb)
        services.Configure<DynamoDbConfiguration>(context.Configuration.GetSection("DynamoDb"));
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
        healthChecks.AddCheck<Amido.Stacks.Data.Documents.CosmosDB.CosmosDbDocumentStorage<Menu>>("CosmosDB");
#endif
    }

    private static void AddCommandHandlers(IServiceCollection services)
    {
        log.Information("Loading implementations of {interface}", typeof(ICommandHandler<,>).Name);
        var definitions = typeof(CreateMenuCommandHandler).Assembly.GetImplementationsOf(typeof(ICommandHandler<,>));
        foreach (var definition in definitions)
        {
            log.Information("Registering '{implementation}' as implementation of '{interface}'",
                definition.implementation.FullName, definition.interfaceVariation.FullName);
            services.AddTransient(definition.interfaceVariation, definition.implementation);
        }
    }

    private static void AddQueryHandlers(IServiceCollection services)
    {
        log.Information("Loading implementations of {interface}", typeof(IQueryHandler<,>).Name);
        var definitions = typeof(GetMenuByIdQueryHandler).Assembly.GetImplementationsOf(typeof(IQueryHandler<,>));
        foreach (var definition in definitions)
        {
            log.Information("Registering '{implementation}' as implementation of '{interface}'",
                definition.implementation.FullName, definition.interfaceVariation.FullName);
            services.AddTransient(definition.interfaceVariation, definition.implementation);
        }
    }
}
