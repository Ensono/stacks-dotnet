using System;
using System.Runtime.Serialization;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Exceptions
{
    [Serializable]
    public class MessageRouteNotDefinedException : ServiceBusExceptionBase
    {
        public MessageRouteNotDefinedException(Guid correlationId, string message, Exception innerException = null) :
            base((int) ExceptionIds.MessageRouteNotDefined, correlationId, message, innerException)
        {
        }

        public MessageRouteNotDefinedException(string message, Exception innerException = null) : base(
            (int) ExceptionIds.MessageRouteNotDefined, message, innerException)
        {
        }
    }
}