using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuUpdatedEventHandler(ILogger<MenuUpdatedEventHandler> logger) 
           : IApplicationEventHandler<MenuUpdatedEvent>
{
    public Task HandleAsync(MenuUpdatedEvent appEvent)
    {
        logger.LogInformation("Executing MenuUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
