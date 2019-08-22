using System;
using Amido.Stacks.Core.Exceptions;
using Amido.Stacks.Core.Operations;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Common.Exceptions
{
    public class DomainRuleViolationException : ApplicationExceptionBase
    {
        private DomainRuleViolationException(
            ExceptionCode exceptionCode,
            OperationCode operationCode,
            Guid correlationId,
            string message,
            Exception domainException
        ) : base((int)exceptionCode, (int)operationCode, correlationId, message, domainException)
        {
        }

        public override int HttpStatusCode => (int)System.Net.HttpStatusCode.BadRequest;

        public static void Raise(OperationCode operationCode, Guid correlationId, Guid menuId, Exception domainException)
        {
            var exception = new DomainRuleViolationException(
                Exceptions.ExceptionCode.MenuDoesNotExist,
                operationCode,
                correlationId,
                $"A domain exception has been raised in the menu '{menuId}'. {domainException.Message}",
                domainException
            );
            exception.Data["MenuId"] = menuId;
            throw exception;
        }

        public static void Raise(IOperationContext context, Guid menuId, Exception domainException)
        {
            Raise((OperationCode)context.OperationCode, context.CorrelationId, menuId, domainException);
        }
    }
}
