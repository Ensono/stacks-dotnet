using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;

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
