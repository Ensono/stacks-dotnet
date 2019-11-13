using System;
using Amido.Stacks.Core.Exceptions;

namespace xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions
{
    [Serializable]
    public class CategoryDoesNotExistException : DomainExceptionBase
    {
        private CategoryDoesNotExistException(
            string message
        ) : base(message)
        {
        }

        public override int ExceptionCode { get; protected set; } = (int)Common.Exceptions.ExceptionCode.CategoryDoesNotExist;

        public static void Raise(Guid menuId, Guid categoryId, string message)
        {
            var exception = new CategoryDoesNotExistException(
                message ?? $"A category with id '{categoryId}' does not exist in the menu '{menuId}'."
            );

            exception.Data["MenuId"] = menuId;
            throw exception;
        }

        public static void Raise(Guid menuId, Guid categoryId)
        {
            Raise(menuId, categoryId, null);
        }
    }
}
