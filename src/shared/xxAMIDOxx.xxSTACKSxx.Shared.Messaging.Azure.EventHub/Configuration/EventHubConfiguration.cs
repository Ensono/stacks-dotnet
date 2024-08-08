namespace Amido.Stacks.Messaging.Azure.EventHub.Configuration
{
    public class EventHubConfiguration
    {
        public EventHubPublisherConfiguration Publisher { get; set; }

        public EventHubConsumerConfiguration Consumer { get; set; }
    }
}
