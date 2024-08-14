using System;
using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders.Routers
{
    public interface IMessageRouter
    {
        Task SendAsync(object message);
        bool Match(Type type);
    }

    public interface ITopicRouter : IMessageRouter { }
    public interface IQueueRouter : IMessageRouter { }
}
