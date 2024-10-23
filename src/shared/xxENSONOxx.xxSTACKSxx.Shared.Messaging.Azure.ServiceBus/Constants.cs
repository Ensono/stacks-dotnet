using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Listeners;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus
{
    internal static class Constants
    {
        internal static class Defaults
        {
            public const string CommandSerializer = nameof(Serializers.JsonMessageSerializer);
            public const string EventSerializer = nameof(Serializers.CloudEventMessageSerializer);

            public const string QueueListenerMessageProcessor = nameof(ServiceBusListenerMessageProcessor<ICommand>);
            public const string TopicListenerMessageProcessor = nameof(ServiceBusListenerMessageProcessor<IApplicationEvent>);
        }
    }

    public enum MessageProperties
    {
        EnclosedMessageType,
        Serializer
    }
}
