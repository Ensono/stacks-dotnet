using System;
using Amido.Stacks.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions
{
    public class CategoryAlreadyExistsException : DomainException
    {
        private CategoryAlreadyExistsException(
            string message
        ) : base(message)
        {
        }

        public override int ExceptionCode => (int)Common.Exceptions.ExceptionCode.CategoryAlreadyExists;

        public static void Raise(Guid menuId, string categoryName, string message)
        {
            var exception = new CategoryAlreadyExistsException(
                message ?? $"A category with name '{categoryName}' already exists in the menu '{menuId}'."
            );
            exception.Data["MenuId"] = menuId;
            throw exception;
        }

        public static void Raise(Guid menuId, string categoryName)
        {
            Raise(menuId, categoryName, null);
        }
    }
}
