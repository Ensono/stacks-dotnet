#if (EventPublisherAwsSns)
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
#endif

#if (EventPublisherAwsSns || EventPublisherEventHub)
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
#endif

#if (EventPublisherEventHub)
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Consumers;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders.Publishers;

#endif

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
     #if (EventPublisherAwsSns)
    /// <summary>
    /// Add the AWS SNS client for IEventConsumer and IApplicationEventPublisher
    /// </summary>
    public static IServiceCollection AddAwsSns(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonSimpleNotificationService>();
        services.AddTransient<IApplicationEventPublisher, EventPublisher>();

        return services;
    }
    #endif

    #if (EventPublisherEventHub)
    public static IServiceCollection AddEventHub(this IServiceCollection services)
    {
        var configuration = GetConfiguration(services);

        var publishersRegistered = services.AddEventHubPublishers(configuration.Publisher);
        var consumersRegistered = services.AddEventHubConsumers(configuration.Consumer);

        if (!publishersRegistered && !consumersRegistered)
        {
            throw new Exception(
                "Unable to register any publishers or consumers for event hub. Make sure the configuration has been setup correctly.");
        }

        return services;
    }

    private static EventHubConfiguration GetConfiguration(IServiceCollection services)
    {
        var config = services.BuildServiceProvider()
            .GetService<IOptions<EventHubConfiguration>>()
            .Value;

        if (config == null || (config.Publisher == null && config.Consumer == null))
        {
            throw new Exception(
                $"Configuration for '{nameof(IOptions<EventHubConfiguration>)}' not found. Ensure the call to 'service.Configure<{nameof(EventHubConfiguration)}>(configuration)' was called and the appsettings contains at least a definition for Publisher or Consumer. ");
        }

        return config;
    }

    private static bool AddEventHubPublishers(this IServiceCollection services,
        EventHubPublisherConfiguration configuration)
    {
        if (configuration == null)
        {
            return false;
        }

        var secretResolver = services.BuildServiceProvider().GetService<ISecretResolver<string>>();
        services.AddSingleton(s => new EventHubProducerClient(
            connectionString: secretResolver.ResolveSecretAsync(configuration.NamespaceConnectionString).Result,
            eventHubName: configuration.EventHubName));

        services.AddTransient<IApplicationEventPublisher, EventHubEventPublisher>();

        return true;
    }

    private static bool AddEventHubConsumers(this IServiceCollection services,
        EventHubConsumerConfiguration configuration)
    {
        if (configuration == null)
        {
            return false;
        }

        var secretResolver = services.BuildServiceProvider().GetService<ISecretResolver<string>>();
        services.AddSingleton(s => new EventProcessorClient(
            checkpointStore: new BlobContainerClient(
                secretResolver.ResolveSecretAsync(configuration.BlobStorageConnectionString).Result,
                configuration.BlobContainerName),
            consumerGroup: EventHubConsumerClient.DefaultConsumerGroupName,
            connectionString: secretResolver.ResolveSecretAsync(configuration.NamespaceConnectionString).Result,
            configuration.EventHubName));

        services.AddTransient<IEventConsumer, EventConsumer>();

        return true;
    }
    #endif
}