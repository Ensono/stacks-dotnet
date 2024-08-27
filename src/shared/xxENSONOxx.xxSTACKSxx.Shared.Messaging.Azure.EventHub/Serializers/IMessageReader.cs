using Azure.Messaging.EventHubs;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Serializers
{
    public interface IMessageReader
    {
        T Read<T>(EventData eventData);
    }
}
