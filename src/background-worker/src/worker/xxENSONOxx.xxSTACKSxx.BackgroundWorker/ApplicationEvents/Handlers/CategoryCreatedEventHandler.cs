using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;

public class CategoryCreatedEventHandler(ILogger<CategoryCreatedEventHandler> logger)
           : IApplicationEventHandler<CategoryCreatedEvent>
{
    public Task HandleAsync(CategoryCreatedEvent appEvent)
    {
        logger.LogInformation("Executing CategoryCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
