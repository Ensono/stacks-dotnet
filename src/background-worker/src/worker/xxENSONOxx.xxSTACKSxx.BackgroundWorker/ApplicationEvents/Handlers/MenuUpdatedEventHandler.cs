using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;

public class MenuUpdatedEventHandler(ILogger<MenuUpdatedEventHandler> logger)
           : IApplicationEventHandler<MenuUpdatedEvent>
{
    public Task HandleAsync(MenuUpdatedEvent appEvent)
    {
        logger.LogInformation("Executing MenuUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
