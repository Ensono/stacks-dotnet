using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class CategoryUpdatedEventHandler(ILogger<CategoryUpdatedEventHandler> log)
    : IApplicationEventHandler<CategoryUpdatedEvent>
{
    public Task HandleAsync(CategoryUpdatedEvent appEvent)
    {
        log.LogInformation($"Executing CategoryUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
