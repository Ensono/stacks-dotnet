using System;
using Amido.Stacks.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions
{
    public class MenuItemAlreadyExistsException : DomainException
    {
        private MenuItemAlreadyExistsException(
            string message
        ) : base(message)
        {
        }

        public override int ExceptionCode => (int)Common.Exceptions.ExceptionCode.MenuItemAlreadyExists;


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
