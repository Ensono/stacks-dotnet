using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Logging;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.Fakes
{
    /// <summary>
    /// This is a dummy event publisher that writes the event to the console
    /// This must be replaced to publish the event to a Topic like Service Bus or Kafka
    /// </summary>
    public class DummyEventPublisher : IApplicationEventPublisher
    {
        readonly ILogger<DummyEventPublisher> logger;

        public DummyEventPublisher(ILogger<DummyEventPublisher> logger)
        {
            this.logger = logger;
        }

        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            logger.LogInformation("Event published: {EventType}", applicationEvent.GetType());

            await Task.CompletedTask;
        }
    }
}
