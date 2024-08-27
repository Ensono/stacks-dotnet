using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuUpdatedEventHandler(ILogger<MenuUpdatedEventHandler> log) : IApplicationEventHandler<MenuUpdatedEvent>
{
    public Task HandleAsync(MenuUpdatedEvent appEvent)
    {
        log.LogInformation($"Executing MenuUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
