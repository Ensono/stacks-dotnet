using Microsoft.Extensions.DependencyInjection;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Secrets;
#if EventPublisherAwsSns
using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Amazon.SimpleNotificationService;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Publishers;
#endif
#if EventPublisherEventHub
using System;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Consumers;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Publishers;
#endif
#if DynamoDb
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;
#endif
#if CosmosDb
using xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;
#endif

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
#if EventPublisherAwsSns
    /// <summary>
    /// Add the AWS SNS client for IEventConsumer and IApplicationEventPublisher
    /// </summary>
    public static IServiceCollection AddAwsSns(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonSimpleNotificationService>();
        services.AddTransient<IApplicationEventPublisher, SnsEventPublisher>();

        return services;
    }
#endif
#if EventPublisherEventHub
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
#if DynamoDb
    /// <summary>
    /// Adds DynamoDB services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <returns>The IServiceCollection with the DynamoDB services added.</returns>
    public static IServiceCollection AddDynamoDB(this IServiceCollection services)
	{
		services.AddAWSService<IAmazonDynamoDB>();
		services.AddTransient<IDynamoDBContext, DynamoDBContext>();
		services.AddTransient(typeof(IDynamoDbObjectStorage<>), typeof(DynamoDbObjectStorage<>));
		services.AddTransient(typeof(IDynamoDbObjectSearch<>), typeof(DynamoDbObjectSearch<>));
		return services;
	}
#endif
#if CosmosDb
    /// <summary>
    /// Add the CosmosDB singleton components for IDocumentStorage<,> and IDocumentSearch<>
    /// This will create one singleton instance per Container(Where the container map to TEntity name)
    /// </summary>
    public static IServiceCollection AddCosmosDB(this IServiceCollection services)
    {
        // CosmosDB components are thread safe and should be singleton to avoid opening new
        // connections on every request, similar to HttpCliient
        services.AddSingleton(typeof(IDocumentStorage<>), typeof(CosmosDbDocumentStorage<>));
        services.AddSingleton(typeof(IDocumentSearch<>), typeof(CosmosDbDocumentStorage<>));
        services.AddSingleton(typeof(CosmosDbDocumentStorage<>));
        return services;
    }
#endif
    /// <summary>
    /// Add the Secret resolver singleton with default secret sources (file and environment)
    /// </summary>
    public static IServiceCollection AddSecrets(this IServiceCollection services)
    {
        services.AddSingleton<ISecretResolver<string>, SecretResolver>();
        return services;
    }
}
