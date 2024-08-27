using System;
using System.Runtime.Serialization;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Exceptions
{
    [Serializable]
    public class MissingHandlerFormAssemblyException : UnrecoverableException
    {
        public MissingHandlerFormAssemblyException(Guid correlationId, string message,
            Exception innerException = null) : base((int)ExceptionIds.MissingHandlerFromAssembly, correlationId,
            message, innerException)
        {
        }

        public MissingHandlerFormAssemblyException(string message,
            Exception innerException = null) : base((int)ExceptionIds.MissingHandlerFromAssembly, message,
            innerException)
        {
        }

        protected MissingHandlerFormAssemblyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
