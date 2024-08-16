using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuUpdatedEventHandler(ILogger<MenuUpdatedEventHandler> log) : IApplicationEventHandler<MenuUpdatedEvent>
{
    public Task HandleAsync(MenuUpdatedEvent appEvent)
    {
        log.LogInformation($"Executing MenuUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
