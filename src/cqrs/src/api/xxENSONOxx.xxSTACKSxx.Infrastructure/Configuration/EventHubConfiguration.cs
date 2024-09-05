namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;

public class EventHubConfiguration
{
    public EventHubPublisherConfiguration Publisher { get; set; }

    public EventHubConsumerConfiguration Consumer { get; set; }
}
