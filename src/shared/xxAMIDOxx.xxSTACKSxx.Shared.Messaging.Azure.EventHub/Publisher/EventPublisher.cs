using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Publisher
{
    public class EventPublisher(
        ILogger<EventPublisher> log,
        EventHubProducerClient eventHubProducerClient)
        : IApplicationEventPublisher
    {
        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            log.LogInformation($"Publishing event {applicationEvent.CorrelationId}");

            EventDataBatch eventDataBatch = await eventHubProducerClient.CreateBatchAsync();

            var eventReading = JsonConvert.SerializeObject(applicationEvent);
            eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(eventReading)));

            try
            {
                await eventHubProducerClient.SendAsync(eventDataBatch);
                log.LogInformation($"Event {applicationEvent.CorrelationId} has been published.");
            }
            catch (Exception exception)
            {
                log.LogError($"Something went wrong. Exception thrown: {exception.Message}");
            }
        }
    }
}
