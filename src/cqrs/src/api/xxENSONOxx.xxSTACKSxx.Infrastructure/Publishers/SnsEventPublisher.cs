#if (EventPublisherAwsSns)
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Logging;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Secrets;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Publishers;

public class SnsEventPublisher(
        IOptions<AwsSnsConfiguration> configuration,
        ISecretResolver<string> secretResolver,
        IAmazonSimpleNotificationService snsClient,
        ILogger<SnsEventPublisher> logger)
        : IApplicationEventPublisher
    {
        private readonly IOptions<AwsSnsConfiguration> configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        private readonly ISecretResolver<string> secretResolver = secretResolver ?? throw new ArgumentNullException(nameof(secretResolver));
        private readonly IAmazonSimpleNotificationService snsClient = snsClient ?? throw new ArgumentNullException(nameof(snsClient));
        private readonly ILogger<SnsEventPublisher> logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Publishes an event message to the configured SNS
        /// </summary>
        /// <param name="applicationEvent">The message object</param>
        /// <returns>Task</returns>
        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            logger.PublishEventRequested(applicationEvent.CorrelationId.ToString());

            var topicArn = await secretResolver.ResolveSecretAsync(configuration.Value.TopicArn);
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            var message = JsonSerializer.Serialize<object>(applicationEvent, jsonOptions);
            var messageRequest = new PublishRequest
            {
                Message = message,
                TopicArn = topicArn
            };

            try
            {
                await snsClient.PublishAsync(messageRequest);

                logger.PublishEventCompleted(applicationEvent.CorrelationId.ToString());
            }
            catch (AmazonSimpleNotificationServiceException exception)
            {
                logger.PublishEventFailed(applicationEvent.CorrelationId.ToString(), exception.Message, exception);
            }
            catch (AmazonClientException exception)
            {
                logger.PublishEventFailed(applicationEvent.CorrelationId.ToString(), exception.Message, exception);
            }
        }
    }
#endif
