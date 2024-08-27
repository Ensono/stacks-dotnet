using System;
using System.Collections.Generic;
using System.Linq;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Serializers;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxENSONOxx.xxSTACKSxx.Listener;

public class StacksListener(IMessageReader msgReader, ILogger<StacksListener> logger)
{
    [FunctionName("StacksListener")]
    public void Run([EventHubTrigger(
        "%EVENTHUB_NAME%",
        Connection = "EVENTHUB_CONNECTIONSTRING")] EventData[] events)
    {
        var exceptions = new List<Exception>();

        foreach (EventData eventData in events)
        {
            try
            {
                var appEvent = msgReader.Read<MenuCreatedEvent>(eventData);
                logger.LogInformation($"Message read. Menu Id: {appEvent?.MenuId}");
                logger.LogInformation($"C# Event Hub trigger function processed an event: {appEvent}");
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }
        }

        if (exceptions.Count > 1)
            throw new AggregateException(exceptions);

        if (exceptions.Count == 1)
            throw exceptions.Single();
    }
}
