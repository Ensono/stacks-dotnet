using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageBuilderFactory
    {
        IMessageBuilder CreateMessageBuilder(string name);
    }
}