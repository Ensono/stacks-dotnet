using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;

public class MenuItemUpdatedEventHandler(ILogger<MenuItemUpdatedEventHandler> logger)
           : IApplicationEventHandler<MenuItemUpdatedEvent>
{
    public Task HandleAsync(MenuItemUpdatedEvent appEvent)
    {
        logger.LogInformation("Executing MenuItemUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
