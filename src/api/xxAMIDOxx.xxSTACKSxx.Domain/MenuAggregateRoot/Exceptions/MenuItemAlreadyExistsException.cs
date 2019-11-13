using System;
using Amido.Stacks.Core.Exceptions;

namespace xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions
{
    [Serializable]
    public class MenuItemAlreadyExistsException : DomainExceptionBase
    {
        private MenuItemAlreadyExistsException(
            string message
        ) : base(message)
        {
        }

        public override int ExceptionCode { get; protected set; } = (int)Common.Exceptions.ExceptionCode.MenuItemAlreadyExists;


        public static void Raise(Guid categoryId, string menuItemName, string message)
        {
            var exception = new MenuItemAlreadyExistsException(
                message ?? $"The item {menuItemName} already exist in the category '{categoryId}'."
            );

            exception.Data["CategoryId"] = categoryId;
            exception.Data["MenuItemName"] = menuItemName;

            throw exception;
        }

        public static void Raise(Guid categoryId, string menuItemName)
        {
            Raise(categoryId, menuItemName, null);
        }
    }
}
