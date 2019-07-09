using System;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Common.Exceptions
{
    //TODO: This shall be moved to stacks, we need to track exceptions as middleware

    public class ApplicationException : Exception
    {
        public ApplicationException(ExceptionId exceptionId, OperationId operationId, string message) : this(exceptionId, operationId, message, null) { }

        public ApplicationException(ExceptionId exceptionId, OperationId operationId, string message, Exception innerException) : base(message, innerException)
        {
            ExceptionId = exceptionId;
            OperationId = operationId;

            Data["ExceptionId"] = (int)exceptionId;
            Data["OperationId"] = (int)operationId;
        }

        public ExceptionId ExceptionId { get; set; }

        public OperationId OperationId { get; set; }
    }
}
