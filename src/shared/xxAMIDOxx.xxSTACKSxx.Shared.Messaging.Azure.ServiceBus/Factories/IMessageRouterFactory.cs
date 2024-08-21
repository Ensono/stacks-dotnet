using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders.Routers;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageRouterFactory
    {
        IMessageRouter Create(MessageRoutingRouterConfiguration configuration);
        IMessageRouter CreateDefault<T>() where T : IMessageRouter;
    }
}