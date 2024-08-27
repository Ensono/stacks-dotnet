using xxENSONOxx.xxSTACKSxx.Shared.Core.Operations;
using Microsoft.Azure.ServiceBus;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers
{
    public interface IMessageBuilder
    {
        Message Build<T>(T body);
    }
}
