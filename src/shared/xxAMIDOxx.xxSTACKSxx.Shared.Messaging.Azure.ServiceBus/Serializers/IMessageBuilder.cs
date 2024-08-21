using xxAMIDOxx.xxSTACKSxx.Shared.Core.Operations;
using Microsoft.Azure.ServiceBus;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers
{
    public interface IMessageBuilder
    {
        Message Build<T>(T body);
    }
}
