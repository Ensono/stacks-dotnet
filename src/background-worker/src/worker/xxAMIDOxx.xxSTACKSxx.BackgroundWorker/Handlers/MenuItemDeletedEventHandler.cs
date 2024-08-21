using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuItemDeletedEventHandler(ILogger<MenuItemDeletedEventHandler> log)
    : IApplicationEventHandler<MenuItemDeletedEvent>
{
    public Task HandleAsync(MenuItemDeletedEvent appEvent)
    {
        log.LogInformation($"Executing MenuItemDeletedEventHandler...");
        return Task.CompletedTask;
    }
}
