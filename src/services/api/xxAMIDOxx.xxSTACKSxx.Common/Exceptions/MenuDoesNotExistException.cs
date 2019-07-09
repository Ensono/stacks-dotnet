using System;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Common.Exceptions
{
    public class MenuDoesNotExistException : ApplicationException
    {
        private MenuDoesNotExistException(
            ExceptionId exceptionId,
            OperationId operationId,
            string message
        ) : base(exceptionId, operationId, message)
        {
        }

        public static void Raise(OperationId operationId, Guid menuId)
        {
            var exception = new MenuDoesNotExistException(ExceptionId.MenuDoesNotExist, operationId, $"A menu with id '{menuId}' does not exist.");
            exception.Data["MenuId"] = menuId;
            throw exception;
        }

        public static void Raise(OperationId operationId, Guid menuId, string message)
        {
            var exception = new MenuDoesNotExistException(ExceptionId.MenuDoesNotExist, operationId, message);
            exception.Data["MenuId"] = menuId;
            throw exception;
        }
    }
}
