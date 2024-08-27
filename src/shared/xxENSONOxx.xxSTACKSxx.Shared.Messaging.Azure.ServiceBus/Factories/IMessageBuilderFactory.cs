using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageBuilderFactory
    {
        IMessageBuilder CreateMessageBuilder(string name);
    }
}
