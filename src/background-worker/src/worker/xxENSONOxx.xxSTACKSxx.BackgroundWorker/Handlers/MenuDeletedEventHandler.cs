using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuDeletedEventHandler(ILogger<MenuDeletedEventHandler> log) : IApplicationEventHandler<MenuDeletedEvent>
{
    public Task HandleAsync(MenuDeletedEvent appEvent)
    {
        log.LogInformation($"Executing MenuDeletedEventHandler...");
        return Task.CompletedTask;
    }
}
