using System;
using Amido.Stacks.Core.Exceptions;
using Amido.Stacks.Core.Operations;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Common.Exceptions
{
    public class MenuDoesNotExistException : ApplicationExceptionBase
    {
        private MenuDoesNotExistException(
            ExceptionCode exceptionCode,
            OperationCode operationCode,
            Guid correlationId,
            string message
        ) : base((int)exceptionCode, (int)operationCode, correlationId, message)
        {
        }

        public static void Raise(OperationCode operationCode, Guid correlationId, Guid menuId, string message)
        {
            var exception = new MenuDoesNotExistException(
                Exceptions.ExceptionCode.MenuDoesNotExist,
                operationCode,
                correlationId,
                message ?? $"A menu with id '{menuId}' does not exist."
            );
            exception.Data["MenuId"] = menuId;
            throw exception;
        }

        public static void Raise(IOperationContext context, Guid menuId)
        {
            Raise((OperationCode)context.OperationCode, context.CorrelationId, menuId, null);
        }
    }
}
