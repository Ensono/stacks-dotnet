using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;

public class CategoryDeletedEventHandler(ILogger<CategoryDeletedEventHandler> logger)
           : IApplicationEventHandler<CategoryDeletedEvent>
{
    public Task HandleAsync(CategoryDeletedEvent appEvent)
    {
        logger.LogInformation("Executing CategoryDeletedEventHandler...");
        return Task.CompletedTask;
    }
}
