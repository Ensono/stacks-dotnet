using System;
using System.Runtime.Serialization;
using Microsoft.Azure.ServiceBus;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Exceptions
{
    [Serializable]
    public class MessageBodyIsNullException : MessageParsingException
    {
        public MessageBodyIsNullException(Guid correlationId, string message, Exception innerException = null) : base(
            (int) ExceptionIds.MessageBodyIsNull, correlationId, message, innerException)
        {
        }

        public MessageBodyIsNullException(string exceptionMessage, Message context, Exception innerException = null) :
            base((int) ExceptionIds.MessageBodyIsNull, exceptionMessage, context, innerException)
        {
        }

        protected MessageBodyIsNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
