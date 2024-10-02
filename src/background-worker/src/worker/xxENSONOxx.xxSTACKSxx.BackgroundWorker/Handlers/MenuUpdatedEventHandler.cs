using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuUpdatedEventHandler(ILogger<MenuUpdatedEventHandler> log) : IApplicationEventHandler<MenuUpdatedEvent>
{
    public Task HandleAsync(MenuUpdatedEvent appEvent)
    {
        log.LogInformation($"Executing MenuUpdatedEventHandler...");
        return Task.CompletedTask;
    }
}
