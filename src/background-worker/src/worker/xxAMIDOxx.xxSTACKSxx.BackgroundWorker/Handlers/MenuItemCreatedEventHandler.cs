using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuItemCreatedEventHandler(ILogger<MenuItemCreatedEventHandler> log)
    : IApplicationEventHandler<MenuItemCreatedEvent>
{
    public Task HandleAsync(MenuItemCreatedEvent appEvent)
    {
        log.LogInformation($"Executing MenuItemCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
