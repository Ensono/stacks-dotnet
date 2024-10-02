using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class CategoryDeletedEventHandler(ILogger<CategoryDeletedEventHandler> log)
    : IApplicationEventHandler<CategoryDeletedEvent>
{
    public Task HandleAsync(CategoryDeletedEvent appEvent)
    {
        log.LogInformation($"Executing CategoryDeletedEventHandler...");
        return Task.CompletedTask;
    }
}
