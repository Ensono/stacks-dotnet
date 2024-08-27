using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders.Routers;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageRouterFactory
    {
        IMessageRouter Create(MessageRoutingRouterConfiguration configuration);
        IMessageRouter CreateDefault<T>() where T : IMessageRouter;
    }
}
