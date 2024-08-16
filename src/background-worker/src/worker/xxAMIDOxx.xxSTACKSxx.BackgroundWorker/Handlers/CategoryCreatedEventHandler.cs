using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class CategoryCreatedEventHandler(ILogger<CategoryCreatedEventHandler> log)
    : IApplicationEventHandler<CategoryCreatedEvent>
{
    public Task HandleAsync(CategoryCreatedEvent appEvent)
    {
        log.LogInformation($"Executing CategoryCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
