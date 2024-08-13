using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class MenuItemDeletedEventHandler : IApplicationEventHandler<MenuItemDeletedEvent>
{
    private readonly ILogger<MenuItemDeletedEventHandler> log;

    public MenuItemDeletedEventHandler(ILogger<MenuItemDeletedEventHandler> log)
    {
        this.log = log;
    }

    public Task HandleAsync(MenuItemDeletedEvent appEvent)
    {
        log.LogInformation($"Executing MenuItemDeletedEventHandler...");
        return Task.CompletedTask;
    }
}
