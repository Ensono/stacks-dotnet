using System;
using Amido.Stacks.Domain;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions
{
    public class CategoryDoesNotExistException : DomainException
    {
        private CategoryDoesNotExistException(
            string message
        ) : base(message)
        {
        }

        public override int ExceptionCode => (int)Common.Exceptions.ExceptionCode.CategoryAlreadyExists;

        public static void Raise(Guid menuId, Guid categoryId, string message)
        {
            var exception = new CategoryDoesNotExistException(
                message ?? $"A category with id '{categoryId}' does not exist in the menu '{menuId}'."
            );

            exception.Data["MenuId"] = menuId;
            throw exception;
        }

        public static void Raise(OperationCode operationCode, Guid menuId, Guid categoryId)
        {
            Raise(menuId, categoryId, null);
        }
    }
}
