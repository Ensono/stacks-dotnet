using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;

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
