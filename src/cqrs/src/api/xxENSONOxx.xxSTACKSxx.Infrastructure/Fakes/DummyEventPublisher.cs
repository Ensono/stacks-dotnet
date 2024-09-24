using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Fakes;

/// <summary>
/// This is a dummy event publisher that writes the event to the console
/// This must be replaced to publish the event to a Topic like Service Bus or Kafka
/// </summary>
public class DummyEventPublisher(ILogger<DummyEventPublisher> logger) : IApplicationEventPublisher
{
    public async Task PublishAsync(IApplicationEvent applicationEvent)
    {
        logger.LogInformation("Event published: {EventType}", applicationEvent.GetType());

        await Task.CompletedTask;
    }
}
