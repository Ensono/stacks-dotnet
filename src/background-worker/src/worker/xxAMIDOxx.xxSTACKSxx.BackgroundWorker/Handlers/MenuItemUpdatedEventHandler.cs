using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuItemUpdatedEventHandler(ILogger<MenuItemUpdatedEventHandler> log)
    : IApplicationEventHandler<MenuItemUpdatedEvent>
{
    public Task HandleAsync(MenuItemUpdatedEvent appEvent)
    {
        log.LogInformation($"Executing MenuItemUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
