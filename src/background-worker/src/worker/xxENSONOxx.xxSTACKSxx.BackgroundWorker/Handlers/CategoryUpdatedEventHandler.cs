using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class CategoryUpdatedEventHandler(ILogger<CategoryUpdatedEventHandler> log)
    : IApplicationEventHandler<CategoryUpdatedEvent>
{
    public Task HandleAsync(CategoryUpdatedEvent appEvent)
    {
        log.LogInformation($"Executing CategoryUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
