using System;
using System.Runtime.Serialization;
using Amido.Stacks.Core.Operations;

namespace Amido.Stacks.Core.Exceptions
{
    public abstract class ApplicationExceptionBase : ApplicationException, IException, IOperationContext
    {
        protected ApplicationExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ApplicationExceptionBase() : base() { }

        public ApplicationExceptionBase(string message) : base(message) { }

        public ApplicationExceptionBase(string message, Exception ex) : base(message, ex) { }


        //The below constructors should be used when in the application context where we have operation code and correlation id

        [Obsolete("ExcetionCode is already implemented from child classes")]
        public ApplicationExceptionBase(int exceptionCode, int operationCode, Guid correlationId, string message) : this(operationCode, correlationId, message, null) { }

        [Obsolete("ExcetionCode is already implemented from child classes")]
        public ApplicationExceptionBase(int exceptionCode, int operationCode, Guid correlationId, string message, Exception innerException) : this(operationCode, correlationId, message, innerException) { }

        public ApplicationExceptionBase(int operationCode, Guid correlationId, string message) : this(operationCode, correlationId, message, null) { }

        public ApplicationExceptionBase(int operationCode, Guid correlationId, string message, Exception ex) : base(message, ex)
        {
            OperationCode = operationCode;
            CorrelationId = correlationId;

            Data["ExceptionCode"] = ExceptionCode;
            Data["OperationCode"] = OperationCode;
            Data["CorrelationId"] = CorrelationId;
        }


        /// <summary>
        /// Unique identifier for this exception type used for aggregation and translation of exception messages
        /// </summary>
        public abstract int ExceptionCode { get; protected set; }

        public int OperationCode { get; }

        public Guid CorrelationId { get; }

        public virtual int HttpStatusCode { get; protected set; } = 500;
    }
}
