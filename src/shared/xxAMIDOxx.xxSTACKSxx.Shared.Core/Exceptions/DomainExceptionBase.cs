using System;
using System.Runtime.Serialization;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Core.Exceptions
{
    public abstract class DomainExceptionBase : ApplicationException, IException
    {
        protected DomainExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public DomainExceptionBase(string message) : base(message)
        {
        }

        public abstract int ExceptionCode { get; protected set; }
    }
}
