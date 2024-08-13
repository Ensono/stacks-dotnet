using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;
using xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events;

namespace xxAMIDOxx.xxSTACKSxx.BackgroundWorker.Handlers;

public class CategoryCreatedEventHandler : IApplicationEventHandler<CategoryCreatedEvent>
{
    private readonly ILogger<CategoryCreatedEventHandler> log;

    public CategoryCreatedEventHandler(ILogger<CategoryCreatedEventHandler> log)
    {
        this.log = log;
    }

    public Task HandleAsync(CategoryCreatedEvent appEvent)
    {
        log.LogInformation($"Executing CategoryCreatedEventHandler...");
        return Task.CompletedTask;
    }
}
