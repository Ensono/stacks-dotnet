using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class CategoryDeletedEventHandler(ILogger<CategoryDeletedEventHandler> log)
    : IApplicationEventHandler<CategoryDeletedEvent>
{
    public Task HandleAsync(CategoryDeletedEvent appEvent)
    {
        log.LogInformation($"Executing CategoryDeletedEventHandler...");
        return Task.CompletedTask;
    }
}
