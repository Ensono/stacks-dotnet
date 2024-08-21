using Azure.Messaging.EventHubs;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Serializers
{
    public interface IMessageReader
    {
        T Read<T>(EventData eventData);
    }
}
