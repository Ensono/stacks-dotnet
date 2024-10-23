using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;

public class MenuItemDeletedEventHandler(ILogger<MenuItemDeletedEventHandler> logger)
           : IApplicationEventHandler<MenuItemDeletedEvent>
{
    public Task HandleAsync(MenuItemDeletedEvent appEvent)
    {
        logger.LogInformation("Executing MenuItemDeletedEventHandler...");
        return Task.CompletedTask;
    }
}
