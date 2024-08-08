using Amido.Stacks.Configuration;
using Amido.Stacks.Messaging.Azure.EventHub.Configuration;
using Amido.Stacks.Messaging.Azure.EventHub.Consumer;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddEventHub(this IServiceCollection services)
        {
            var configuration = GetConfiguration(services);

            var publishersRegistered = services.AddEventHubPublishers(configuration.Publisher);
            var consumersRegistered = services.AddEventHubConsumers(configuration.Consumer);

            if (!publishersRegistered && !consumersRegistered)
            {
                throw new Exception("Unable to register any publishers or consumers for event hub. Make sure the configuration has been setup correctly.");
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
                throw new Exception($"Configuration for '{nameof(IOptions<EventHubConfiguration>)}' not found. Ensure the call to 'service.Configure<{nameof(EventHubConfiguration)}>(configuration)' was called and the appsettings contains at least a definition for Publisher or Consumer. ");
            }

            return config;
        }

        private static bool AddEventHubPublishers(this IServiceCollection services, EventHubPublisherConfiguration configuration)
        {
            if (configuration == null)
            {
                return false;
            }

            var secretResolver = services.BuildServiceProvider().GetService<ISecretResolver<string>>();
            services.AddSingleton(s => new EventHubProducerClient(
                connectionString: secretResolver.ResolveSecretAsync(configuration.NamespaceConnectionString).Result,
                eventHubName: configuration.EventHubName));

            //services.TryAddTransient<IEventHubProducerClientFactory, EventHubProducerClientFactory>();

            return true;
        }

        private static bool AddEventHubConsumers(this IServiceCollection services, EventHubConsumerConfiguration configuration)
        {
            if (configuration == null)
            {
                return false;
            }

            var secretResolver = services.BuildServiceProvider().GetService<ISecretResolver<string>>();
            services.AddSingleton(s => new EventProcessorClient(
                checkpointStore: new BlobContainerClient(secretResolver.ResolveSecretAsync(configuration.BlobStorageConnectionString).Result, configuration.BlobContainerName),
                consumerGroup: EventHubConsumerClient.DefaultConsumerGroupName,
                connectionString: secretResolver.ResolveSecretAsync(configuration.NamespaceConnectionString).Result,
                configuration.EventHubName));

            services.AddTransient<IEventConsumer, EventConsumer>();

            return true;
        }
    }
}
