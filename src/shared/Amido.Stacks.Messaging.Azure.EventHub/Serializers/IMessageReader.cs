using Azure.Messaging.EventHubs;

namespace Amido.Stacks.Messaging.Azure.EventHub.Serializers
{
    public interface IMessageReader
    {
        T Read<T>(EventData eventData);
    }
}
