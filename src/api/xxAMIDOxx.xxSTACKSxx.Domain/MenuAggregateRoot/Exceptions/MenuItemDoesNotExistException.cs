using System;
using Amido.Stacks.Core.Exceptions;

namespace xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions
{
    [Serializable]
    public class MenuItemDoesNotExistException : DomainExceptionBase
    {
        private MenuItemDoesNotExistException(
            string message
        ) : base(message)
        {
        }


        public override int ExceptionCode { get; protected set; } = (int)Common.Exceptions.ExceptionCode.MenuItemDoesNotExist;

        public static void Raise(Guid categoryId, Guid menuItemId, string message)
        {
            var exception = new MenuItemDoesNotExistException(
                message ?? $"The item {menuItemId} does not exist in the category '{categoryId}'."
            );

            exception.Data["CategoryId"] = categoryId;
            exception.Data["MenuItemId"] = menuItemId;

            throw exception;
        }

        public static void Raise(Guid categoryId, Guid menuItemId)
        {
            Raise(categoryId, menuItemId, null);
        }
    }
}
