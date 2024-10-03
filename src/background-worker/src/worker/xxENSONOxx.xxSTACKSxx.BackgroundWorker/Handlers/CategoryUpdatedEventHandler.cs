using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class CategoryUpdatedEventHandler(ILogger<CategoryUpdatedEventHandler> logger)
           : IApplicationEventHandler<CategoryUpdatedEvent>
{
    public Task HandleAsync(CategoryUpdatedEvent appEvent)
    {
        logger.LogInformation("Executing CategoryUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
