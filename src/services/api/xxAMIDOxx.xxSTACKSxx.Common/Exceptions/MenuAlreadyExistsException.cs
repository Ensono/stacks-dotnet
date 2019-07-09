using System;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Common.Exceptions
{
    public class MenuAlreadyExistsException : ApplicationException
    {
        private MenuAlreadyExistsException(
            ExceptionId exceptionId,
            OperationId operationId,
            string message
        ) : base(exceptionId, operationId, message)
        {
        }

        public static void Raise(OperationId operationId, Guid menuId)
        {
            var exception = new MenuAlreadyExistsException(ExceptionId.MenuAlreadyExists, operationId, $"A menu with id '{menuId}' already exists.");
            exception.Data["MenuId"] = menuId;
            throw exception;
        }

        public static void Raise(OperationId operationId, Guid menuId, string message)
        {
            var exception = new MenuAlreadyExistsException(ExceptionId.MenuAlreadyExists, operationId, message);
            exception.Data["MenuId"] = menuId;
            throw exception;
        }

    }
}
