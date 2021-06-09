using System;
using Amido.Stacks.Core.Exceptions;
using Amido.Stacks.Core.Operations;
using Snyk.Fixes.Common.Operations;

namespace Snyk.Fixes.Common.Exceptions
{
    [Serializable]
    public class localhostAlreadyExistsException : ApplicationExceptionBase
    {
        private localhostAlreadyExistsException(
            ExceptionCode exceptionCode,
            OperationCode operationCode,
            Guid correlationId,
            string message
        ) : base((int)exceptionCode, (int)operationCode, correlationId, message)
        {
            HttpStatusCode = (int)ExceptionCodeToHttpStatusCodeConverter.GetHttpStatusCode((int)exceptionCode);
        }

        public override int ExceptionCode { get; protected set; }

        public static void Raise(OperationCode operationCode, Guid correlationId, Guid localhostId, string message)
        {
            var exception = new localhostAlreadyExistsException(
                Exceptions.ExceptionCode.localhostAlreadyExists,
                operationCode,
                correlationId,
                message ?? $"A localhost with id '{localhostId}' already exists."
            );
            exception.Data["localhostId"] = localhostId;
            throw exception;
        }

        public static void Raise(IOperationContext context, Guid localhostId)
        {
            Raise((OperationCode)context.OperationCode, context.CorrelationId, localhostId, null);
        }
    }
}
