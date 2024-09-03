using System;
using System.Diagnostics;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Queries;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CommandHandlers;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.Application.QueryHandlers;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Fakes;
#if (EventPublisherAwsSns)
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.AWS.SNS;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.AWS.SNS.Extensions;
#endif
#if (CosmosDb || DynamoDb)
using xxENSONOxx.xxSTACKSxx.Shared.DynamoDB;
using xxENSONOxx.xxSTACKSxx.Shared.DynamoDB.Extensions;
using Amazon.DynamoDBv2;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Repositories;
using xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.Extensions;
#endif

namespace xxENSONOxx.xxSTACKSxx.Infrastructure;

public static class DependencyRegistration
{
    static readonly ILogger log = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("DependencyRegistration");

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
    public static void ConfigureProductionDependencies(IConfiguration configuration, IServiceCollection services)
    {
        services.AddSecrets();

#if (EventPublisherServiceBus)
        services.Configure<xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration.ServiceBusConfiguration>(configuration.GetSection("ServiceBusConfiguration"));
        services.AddServiceBus();
        services.AddTransient<IApplicationEventPublisher, xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders.Publishers.EventPublisher>();
#elif (EventPublisherEventHub)
        services.Configure<xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Configuration.EventHubConfiguration>(configuration.GetSection("EventHubConfiguration"));
        services.AddEventHub();
        services.AddTransient<IApplicationEventPublisher, xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Publisher.EventPublisher>();
#elif (EventPublisherAwsSns)
        services.Configure<AwsSnsConfiguration>(configuration.GetSection("AwsSnsConfiguration"));
        services.AddAwsSns(configuration);
        services.AddTransient<IApplicationEventPublisher, xxENSONOxx.xxSTACKSxx.Shared.Messaging.AWS.SNS.Publisher.EventPublisher>();
#elif (EventPublisherNone)
        services.AddTransient<IApplicationEventPublisher, DummyEventPublisher>();
#else
        services.AddTransient<IApplicationEventPublisher, DummyEventPublisher>();
#endif

#if (CosmosDb)
        services.Configure<xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.CosmosDbConfiguration>(configuration.GetSection("CosmosDb"));
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
        healthChecks.AddCheck<xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.CosmosDbDocumentStorage<Menu>>("CosmosDB");
#endif
        Debug.WriteLine("ConfigureProductionDependencies");
    }
    
    private static void AddCommandHandlers(IServiceCollection services)
    {
        log.LogInformation("Loading implementations of {interface}", typeof(ICommandHandler<,>).Name);
        
        services.AddTransient<ICommandHandler<CreateCategory, Guid>, CreateCategoryCommandHandler>();
        services.AddTransient<ICommandHandler<DeleteCategory, bool>, DeleteCategoryCommandHandler>();
        services.AddTransient<ICommandHandler<UpdateCategory, bool>, UpdateCategoryCommandHandler>();
        services.AddTransient<ICommandHandler<CreateMenu, Guid>, CreateMenuCommandHandler>();
        services.AddTransient<ICommandHandler<DeleteMenu, bool>, DeleteMenuCommandHandler>();
        services.AddTransient<ICommandHandler<UpdateMenu, bool>, UpdateMenuCommandHandler>();
        services.AddTransient<ICommandHandler<CreateMenuItem, Guid>, CreateMenuItemCommandHandler>();
        services.AddTransient<ICommandHandler<DeleteMenuItem, bool>, DeleteMenuItemCommandHandler>();
        services.AddTransient<ICommandHandler<UpdateMenuItem, bool>, UpdateMenuItemCommandHandler>();
    }

    private static void AddQueryHandlers(IServiceCollection services)
    {
        log.LogInformation("Loading implementations of {interface}", typeof(IQueryHandler<,>).Name);

        services.AddTransient<IQueryHandler<GetMenuById, Menu>, GetMenuByIdQueryHandler>();
        services.AddTransient<IQueryHandler<SearchMenu, SearchMenuResult>, SearchMenuQueryHandler>();
    }
}
