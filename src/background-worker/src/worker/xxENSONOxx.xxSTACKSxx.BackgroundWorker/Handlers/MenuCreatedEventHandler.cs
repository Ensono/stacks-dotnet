using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuCreatedEventHandler(ILogger<MenuCreatedEventHandler> log) : IApplicationEventHandler<MenuCreatedEvent>
{
    public Task HandleAsync(MenuCreatedEvent appEvent)
    {
        log.LogInformation($"Executing MenuCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
