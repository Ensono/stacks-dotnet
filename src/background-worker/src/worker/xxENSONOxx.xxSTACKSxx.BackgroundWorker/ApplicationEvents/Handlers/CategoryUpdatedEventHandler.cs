using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;

public class CategoryUpdatedEventHandler(ILogger<CategoryUpdatedEventHandler> logger)
           : IApplicationEventHandler<CategoryUpdatedEvent>
{
    public Task HandleAsync(CategoryUpdatedEvent appEvent)
    {
        logger.LogInformation("Executing CategoryUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
