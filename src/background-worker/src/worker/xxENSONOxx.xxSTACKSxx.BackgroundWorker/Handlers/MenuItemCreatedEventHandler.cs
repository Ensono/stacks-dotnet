using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;

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
