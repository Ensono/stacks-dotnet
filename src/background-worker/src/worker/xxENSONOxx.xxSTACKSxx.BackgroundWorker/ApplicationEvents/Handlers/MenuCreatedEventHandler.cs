using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;

public class MenuCreatedEventHandler(ILogger<MenuCreatedEventHandler> logger, IConfiguration configuration)
           : IApplicationEventHandler<MenuCreatedEvent>
{
    public Task HandleAsync(MenuCreatedEvent appEvent)
    {
        string topicName = configuration["ServiceBusConfiguration:Listener:Topics:0:Name"];
        string subscriptionName = configuration["ServiceBusConfiguration:Listener:Topics:0:SubscriptionName"];
        logger.LogInformation($"Using Topic {topicName} and Subscription {subscriptionName}");
        logger.LogInformation("Executing MenuCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
