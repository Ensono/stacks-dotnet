using System.Text.Json;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Shared.Configuration;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.AWS.SNS.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.AWS.SNS.Publisher
{
    /// <summary>
    /// Class implementing the ability to publish an event to AWS SNS
    /// </summary>
    public class EventPublisher(
        IOptions<AwsSnsConfiguration> configuration,
        ISecretResolver<string> secretResolver,
        IAmazonSimpleNotificationService snsClient,
        ILogger<EventPublisher> logger)
        : IApplicationEventPublisher
    {
        private readonly IOptions<AwsSnsConfiguration> configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        private readonly ISecretResolver<string> secretResolver = secretResolver ?? throw new ArgumentNullException(nameof(secretResolver));
        private readonly IAmazonSimpleNotificationService snsClient = snsClient ?? throw new ArgumentNullException(nameof(snsClient));
        private readonly ILogger<EventPublisher> logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Publishes an event message to the configured SNS
        /// </summary>
        /// <param name="applicationEvent">The message object</param>
        /// <returns>Task</returns>
        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            logger.PublishEventRequested(applicationEvent.CorrelationId.ToString());

            var topicArn = await secretResolver.ResolveSecretAsync(configuration.Value.TopicArn);
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
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
}
