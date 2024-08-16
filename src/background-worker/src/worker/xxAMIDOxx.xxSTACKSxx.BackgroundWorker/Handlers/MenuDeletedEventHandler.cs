using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuDeletedEventHandler(ILogger<MenuDeletedEventHandler> log) : IApplicationEventHandler<MenuDeletedEvent>
{
    public Task HandleAsync(MenuDeletedEvent appEvent)
    {
        log.LogInformation($"Executing MenuDeletedEventHandler...");
        return Task.CompletedTask;
    }
}
