using System;
using System.Runtime.Serialization;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Exceptions
{
    [Serializable]
    public class MissingQueueConfigurationException : ServiceBusExceptionBase
    {
        public MissingQueueConfigurationException(Guid correlationId, string message, Exception innerException = null) : base((int)ExceptionIds.MissingQueueConfiguration, correlationId, message, innerException)
        {
        }

        public MissingQueueConfigurationException(string message, Exception innerException = null) : base((int)ExceptionIds.MissingQueueConfiguration, message, innerException)
        {
        }
    }
}
