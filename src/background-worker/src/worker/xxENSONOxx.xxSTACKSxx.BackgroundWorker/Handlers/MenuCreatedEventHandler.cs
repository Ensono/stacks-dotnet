using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuCreatedEventHandler(ILogger<MenuCreatedEventHandler> log) : IApplicationEventHandler<MenuCreatedEvent>
{
    public Task HandleAsync(MenuCreatedEvent appEvent)
    {
        log.LogInformation($"Executing MenuCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
