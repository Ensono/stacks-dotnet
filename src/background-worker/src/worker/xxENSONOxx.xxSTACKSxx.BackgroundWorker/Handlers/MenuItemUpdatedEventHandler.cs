using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuItemUpdatedEventHandler(ILogger<MenuItemUpdatedEventHandler> log)
    : IApplicationEventHandler<MenuItemUpdatedEvent>
{
    public Task HandleAsync(MenuItemUpdatedEvent appEvent)
    {
        log.LogInformation($"Executing MenuItemUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
