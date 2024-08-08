using System;
using System.Runtime.Serialization;

namespace Amido.Stacks.Core.Exceptions
{
    public abstract class InfrastructureExceptionBase : Exception, IException
    {
        public InfrastructureExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public InfrastructureExceptionBase() : this(null, null) { }

        public InfrastructureExceptionBase(string message) : this(message, null) { }

        [Obsolete("ExceptionCode is already implemented from child classes")]
        public InfrastructureExceptionBase(int exceptionCode, string message) : this(message, null) { }

        [Obsolete("ExceptionCode is already implemented from child classes")]
        public InfrastructureExceptionBase(int exceptionCode, string message, Exception innerException) : this(message, innerException) { }

        public InfrastructureExceptionBase(string message, Exception ex) : base(message, ex)
        {
            Data["ExceptionCode"] = ExceptionCode;
        }

        public abstract int ExceptionCode { get; protected set; } //this should not have a set

        public virtual int HttpStatusCode { get; protected set; } = 500; //this should not have a set
    }
}
