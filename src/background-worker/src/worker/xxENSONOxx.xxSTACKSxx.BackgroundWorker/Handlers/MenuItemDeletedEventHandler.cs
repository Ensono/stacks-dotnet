using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuItemDeletedEventHandler(ILogger<MenuItemDeletedEventHandler> log)
    : IApplicationEventHandler<MenuItemDeletedEvent>
{
    public Task HandleAsync(MenuItemDeletedEvent appEvent)
    {
        log.LogInformation($"Executing MenuItemDeletedEventHandler...");
        return Task.CompletedTask;
    }
}
