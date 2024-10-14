using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;

public class MenuDeletedEventHandler(ILogger<MenuDeletedEventHandler> logger)
           : IApplicationEventHandler<MenuDeletedEvent>
{
    public Task HandleAsync(MenuDeletedEvent appEvent)
    {
        logger.LogInformation("Executing MenuDeletedEventHandler...");
        return Task.CompletedTask;
    }
}
