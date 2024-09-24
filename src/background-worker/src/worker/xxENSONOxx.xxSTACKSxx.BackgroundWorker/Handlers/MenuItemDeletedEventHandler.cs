using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;

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
