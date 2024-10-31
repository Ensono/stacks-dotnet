using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;

namespace xxENSONOxx.xxSTACKSxx.Listener;

public class StacksListener(ILogger<StacksListener> logger)
{
    [Function(nameof(StacksListener))]
    public void Run([ServiceBusTrigger(
        "%TOPIC_NAME%",
        "%SUBSCRIPTION_NAME%",
        Connection = "SERVICEBUS_CONNECTIONSTRING")] ServiceBusReceivedMessage serviceBusReceivedMessage)
    {
        var appEvent = JsonConvert.DeserializeObject<StacksCloudEvent<MenuCreatedEvent>>(Encoding.UTF8.GetString(serviceBusReceivedMessage.Body));

        // TODO: work with appEvent
        logger.LogInformation($"Message read. Menu Id: {appEvent?.Data?.MenuId}");
        logger.LogInformation($"C# ServiceBus topic trigger function processed message: {appEvent}");
    }
}
