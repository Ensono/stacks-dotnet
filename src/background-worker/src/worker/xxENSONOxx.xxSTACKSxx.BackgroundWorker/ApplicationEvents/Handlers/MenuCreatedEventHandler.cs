using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;

public class MenuCreatedEventHandler(ILogger<MenuCreatedEventHandler> logger)
           : IApplicationEventHandler<MenuCreatedEvent>
{
    public Task HandleAsync(MenuCreatedEvent appEvent)
    {
        logger.LogInformation("Executing MenuCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
