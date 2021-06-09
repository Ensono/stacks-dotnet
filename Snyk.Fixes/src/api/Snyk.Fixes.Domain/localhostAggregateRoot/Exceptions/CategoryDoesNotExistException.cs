using System;
using Amido.Stacks.Core.Exceptions;

namespace Snyk.Fixes.Domain.localhostAggregateRoot.Exceptions
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

        public static void Raise(Guid localhostId, Guid categoryId, string message)
        {
            var exception = new CategoryDoesNotExistException(
                message ?? $"A category with id '{categoryId}' does not exist in the localhost '{localhostId}'."
            );

            exception.Data["localhostId"] = localhostId;
            throw exception;
        }

        public static void Raise(Guid localhostId, Guid categoryId)
        {
            Raise(localhostId, categoryId, null);
        }
    }
}
