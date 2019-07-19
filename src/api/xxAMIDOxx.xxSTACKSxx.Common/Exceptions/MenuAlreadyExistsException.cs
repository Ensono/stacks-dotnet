using System;
using Amido.Stacks.Core.Exceptions;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Common.Exceptions
{
    public class MenuAlreadyExistsException : ApplicationExceptionBase
    {
        private MenuAlreadyExistsException(
            ExceptionCode exceptionCode,
            OperationCode operationCode,
            Guid correlationId,
            string message
        ) : base((int)exceptionCode, (int)operationCode, correlationId, message)
        {
        }

        public static void Raise(Guid correlationId, OperationCode operationCode, Guid menuId, string message)
        {
            var exception = new MenuAlreadyExistsException(Exceptions.ExceptionCode.MenuAlreadyExists, operationCode, correlationId, message);
            exception.Data["MenuId"] = menuId;
            throw exception;
        }

        public static void Raise(Guid correlationId, OperationCode operationCode, Guid menuId)
        {
            Raise(correlationId, operationCode, menuId, $"A menu with id '{menuId}' already exists.");
        }


    }
}
