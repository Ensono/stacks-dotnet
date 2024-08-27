using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

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
