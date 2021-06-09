using System;
using Amido.Stacks.Core.Exceptions;

namespace Snyk.Fixes.Domain.localhostAggregateRoot.Exceptions
{
    [Serializable]
    public class localhostItemDoesNotExistException : DomainExceptionBase
    {
        private localhostItemDoesNotExistException(
            string message
        ) : base(message)
        {
        }


        public override int ExceptionCode { get; protected set; } = (int)Common.Exceptions.ExceptionCode.localhostItemDoesNotExist;

        public static void Raise(Guid categoryId, Guid localhostItemId, string message)
        {
            var exception = new localhostItemDoesNotExistException(
                message ?? $"The item {localhostItemId} does not exist in the category '{categoryId}'."
            );

            exception.Data["CategoryId"] = categoryId;
            exception.Data["localhostItemId"] = localhostItemId;

            throw exception;
        }

        public static void Raise(Guid categoryId, Guid localhostItemId)
        {
            Raise(categoryId, localhostItemId, null);
        }
    }
}
