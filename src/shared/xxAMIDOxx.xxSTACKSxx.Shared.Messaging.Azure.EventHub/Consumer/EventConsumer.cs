using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Amido.Stacks.Messaging.Azure.EventHub.Consumer
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ILogger<EventConsumer> _log;
        private readonly EventProcessorClient _eventProcessorClient;

        public EventConsumer(
            ILogger<EventConsumer> log,
            EventProcessorClient eventProcessorClient)
        {
            _log = log;
            _eventProcessorClient = eventProcessorClient;
        }

        public async Task ProcessAsync()
        {
            _eventProcessorClient.ProcessEventAsync += ProcessEventHandler;
            _eventProcessorClient.ProcessErrorAsync += ProcessErrorHandler;

            await _eventProcessorClient.StartProcessingAsync();

            await Task.Delay(TimeSpan.FromSeconds(30));

            await _eventProcessorClient.StopProcessingAsync();
        }

        private async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            _log.LogInformation("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            _log.LogError($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            _log.LogError(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}