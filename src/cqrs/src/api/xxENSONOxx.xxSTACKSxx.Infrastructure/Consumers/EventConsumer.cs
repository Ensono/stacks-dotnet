#if (EventPublisherEventHub)

using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Extensions.Logging;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Consumers;

public class EventConsumer(
    ILogger<EventConsumer> log,
    EventProcessorClient eventProcessorClient)
    : IEventConsumer
{
    public async Task ProcessAsync()
    {
        eventProcessorClient.ProcessEventAsync += ProcessEventHandler;
        eventProcessorClient.ProcessErrorAsync += ProcessErrorHandler;

        await eventProcessorClient.StartProcessingAsync();

        await Task.Delay(TimeSpan.FromSeconds(30));

        await eventProcessorClient.StopProcessingAsync();
    }

    private async Task ProcessEventHandler(ProcessEventArgs eventArgs)
    {
        log.LogInformation("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
        await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
    }

    private Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
    {
        log.LogError($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
        log.LogError(eventArgs.Exception.Message);
        return Task.CompletedTask;
    }
}
#endif
