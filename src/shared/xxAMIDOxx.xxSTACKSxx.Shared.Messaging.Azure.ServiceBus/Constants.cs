using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Listeners;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus
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
