using System;
using Amido.Stacks.Core.Exceptions;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Common.Exceptions
{
    public class MenuAlreadyExistsException : ApplicationExceptionBase
    {
        private MenuAlreadyExistsException(
            ExceptionCode exceptionCode,
            OperationId operationId,
            string message
        ) : base((int)exceptionCode, (int)operationId, message)
        {
        }

        public static void Raise(OperationId operationId, Guid menuId)
        {
            var exception = new MenuAlreadyExistsException(Exceptions.ExceptionCode.MenuAlreadyExists, operationId, $"A menu with id '{menuId}' already exists.");
            exception.Data["MenuId"] = menuId;
            throw exception;
        }

        public static void Raise(OperationId operationId, Guid menuId, string message)
        {
            var exception = new MenuAlreadyExistsException(Exceptions.ExceptionCode.MenuAlreadyExists, operationId, message);
            exception.Data["MenuId"] = menuId;
            throw exception;
        }

    }
}
