using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

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
