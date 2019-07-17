using System;
using Amido.Stacks.Core.Exceptions;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Common.Exceptions
{
    public class MenuDoesNotExistException : ApplicationExceptionBase
    {
        private MenuDoesNotExistException(
            ExceptionCode exceptionCode,
            OperationId operationId,
            string message
        ) : base((int)exceptionCode, (int)operationId, message)
        {
        }

        public static void Raise(OperationId operationId, Guid menuId)
        {
            var exception = new MenuDoesNotExistException(Exceptions.ExceptionCode.MenuDoesNotExist,
                                                          operationId,
                                                          $"A menu with id '{menuId}' does not exist.");
            exception.Data["MenuId"] = menuId;
            throw exception;
        }

        public static void Raise(OperationId operationId, Guid menuId, string message)
        {
            var exception = new MenuDoesNotExistException(Exceptions.ExceptionCode.MenuDoesNotExist, operationId, message);
            exception.Data["MenuId"] = menuId;
            throw exception;
        }
    }
}
