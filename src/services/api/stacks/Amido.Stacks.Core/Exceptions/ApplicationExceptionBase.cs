using System;

namespace Amido.Stacks.Core.Exceptions
{
    public abstract class ApplicationExceptionBase : Exception
    {
        public ApplicationExceptionBase(int exceptionCode, int operationId, string message) : this(exceptionCode, operationId, message, null) { }

        public ApplicationExceptionBase(int exceptionCode, int operationId, string message, Exception innerException) : base(message, innerException)
        {
            ExceptionCode = exceptionCode;
            OperationId = operationId;

            Data["ExceptionId"] = exceptionCode;
            Data["OperationId"] = operationId;
        }

        public int? ExceptionCode { get; set; }

        public int? OperationId { get; set; }
    }
}
