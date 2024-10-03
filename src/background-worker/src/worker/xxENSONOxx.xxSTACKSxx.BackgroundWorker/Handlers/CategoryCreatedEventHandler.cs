using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class CategoryCreatedEventHandler(ILogger<CategoryCreatedEventHandler> logger)
           : IApplicationEventHandler<CategoryCreatedEvent>
{
    public Task HandleAsync(CategoryCreatedEvent appEvent)
    {
        logger.LogInformation("Executing CategoryCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
