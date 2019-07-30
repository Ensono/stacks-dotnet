using System;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.Fakes
{
    /// <summary>
    /// This is a dummy event publisher that writes the event to the console
    /// This must be replaced to publish the event to a Topic like Service Bus or Kafka
    /// </summary>
    public class DummyEventPublisher : IApplicationEventPublisher
    {
        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            Console.WriteLine($"Event published: {applicationEvent.GetType()}");
            await Task.CompletedTask;
        }
    }
}
