using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;

public class MenuItemCreatedEventHandler(ILogger<MenuItemCreatedEventHandler> logger)
           : IApplicationEventHandler<MenuItemCreatedEvent>
{
    public Task HandleAsync(MenuItemCreatedEvent appEvent)
    {
        logger.LogInformation("Executing MenuItemCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
