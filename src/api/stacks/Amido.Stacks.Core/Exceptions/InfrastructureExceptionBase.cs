using System;
using Amido.Stacks.Core.Operations;

namespace Amido.Stacks.Core.Exceptions
{
    public class InfrastructureExceptionBase : Exception, IException, IOperationContext
    {
        public InfrastructureExceptionBase(int exceptionCode, int operationCode, Guid correlationId, string message) : this(exceptionCode, operationCode, correlationId, message, null) { }

        public InfrastructureExceptionBase(int exceptionCode, int operationCode, Guid correlationId, string message, Exception innerException) : base(message, innerException)
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

    }
}
