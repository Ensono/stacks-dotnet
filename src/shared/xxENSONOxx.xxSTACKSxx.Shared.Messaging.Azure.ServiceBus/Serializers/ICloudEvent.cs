using System;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers
{
    public interface ICloudEvent
    {
        string Id { get; }
        string Subject { get; }
        DateTime? Time { get; }
    }
}
