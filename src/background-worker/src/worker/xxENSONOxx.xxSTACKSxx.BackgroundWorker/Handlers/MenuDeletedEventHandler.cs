using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuDeletedEventHandler(ILogger<MenuDeletedEventHandler> log) : IApplicationEventHandler<MenuDeletedEvent>
{
    public Task HandleAsync(MenuDeletedEvent appEvent)
    {
        log.LogInformation($"Executing MenuDeletedEventHandler...");
        return Task.CompletedTask;
    }
}
