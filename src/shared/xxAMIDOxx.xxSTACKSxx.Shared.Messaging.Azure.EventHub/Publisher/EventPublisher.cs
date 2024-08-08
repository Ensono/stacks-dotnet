using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Amido.Stacks.Messaging.Azure.EventHub.Publisher
{
    public class EventPublisher : IApplicationEventPublisher
    {
        private readonly ILogger<EventPublisher> _log;
        private readonly EventHubProducerClient _eventHubProducerClient;

        public EventPublisher(
            ILogger<EventPublisher> log, 
            EventHubProducerClient eventHubProducerClient)
        {
            _log = log;
            _eventHubProducerClient = eventHubProducerClient;
        }

        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            _log.LogInformation($"Publishing event {applicationEvent.CorrelationId}");

            EventDataBatch eventDataBatch = await _eventHubProducerClient.CreateBatchAsync();

            var eventReading = JsonConvert.SerializeObject(applicationEvent);
            eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(eventReading)));

            try
            {
                await _eventHubProducerClient.SendAsync(eventDataBatch);
                _log.LogInformation($"Event {applicationEvent.CorrelationId} has been published.");
            }
            catch (Exception exception)
            {
                _log.LogError($"Something went wrong. Exception thrown: {exception.Message}");
            }
        }
    }
}
