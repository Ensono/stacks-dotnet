using System;
using Amido.Stacks.Core.Exceptions;

namespace Snyk.Fixes.Domain.localhostAggregateRoot.Exceptions
{
    [Serializable]
    public class CategoryAlreadyExistsException : DomainExceptionBase
    {
        private CategoryAlreadyExistsException(
            string message
        ) : base(message)
        {
        }

        public override int ExceptionCode { get; protected set; } = (int)Common.Exceptions.ExceptionCode.CategoryAlreadyExists;

        public static void Raise(Guid localhostId, string categoryName, string message)
        {
            var exception = new CategoryAlreadyExistsException(
                message ?? $"A category with name '{categoryName}' already exists in the localhost '{localhostId}'."
            );
            exception.Data["localhostId"] = localhostId;
            throw exception;
        }

        public static void Raise(Guid localhostId, string categoryName)
        {
            Raise(localhostId, categoryName, null);
        }
    }
}
