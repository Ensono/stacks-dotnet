using System;
using Amido.Stacks.Core.Operations;

namespace Amido.Stacks.Core.Exceptions
{
    public abstract class ApplicationExceptionBase : Exception, IException, IOperationContext
    {
        public ApplicationExceptionBase(int exceptionCode, int operationCode, Guid correlationId, string message) : this(exceptionCode, operationCode, correlationId, message, null) { }

        public ApplicationExceptionBase(int exceptionCode, int operationCode, Guid correlationId, string message, Exception innerException) : base(message, innerException)
        {
            ExceptionCode = exceptionCode;
            OperationCode = operationCode;
            CorrelationId = correlationId;

            Data["ExceptionCode"] = ExceptionCode;
            Data["OperationCode"] = OperationCode;
            Data["CorrelationId"] = CorrelationId;
        }

        public int ExceptionCode { get; }

        public int OperationCode { get; }

        public Guid CorrelationId { get; }

        public virtual int HttpStatusCode => 500;
    }
}
