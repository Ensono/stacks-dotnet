using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuItemCreatedEventHandler(ILogger<MenuItemCreatedEventHandler> log)
    : IApplicationEventHandler<MenuItemCreatedEvent>
{
    public Task HandleAsync(MenuItemCreatedEvent appEvent)
    {
        log.LogInformation($"Executing MenuItemCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
