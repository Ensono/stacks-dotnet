using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Listeners;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IServiceBusListenerFactory
    {
        IMessageListener Create<T>(ServiceBusQueueListenerConfiguration configuration);
    }
}