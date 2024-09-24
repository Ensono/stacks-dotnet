using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class CategoryCreatedEventHandler(ILogger<CategoryCreatedEventHandler> log)
    : IApplicationEventHandler<CategoryCreatedEvent>
{
    public Task HandleAsync(CategoryCreatedEvent appEvent)
    {
        log.LogInformation($"Executing CategoryCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
