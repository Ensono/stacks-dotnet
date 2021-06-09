using System;
using Amido.Stacks.Core.Exceptions;

namespace Snyk.Fixes.Domain.localhostAggregateRoot.Exceptions
{
    [Serializable]
    public class localhostItemAlreadyExistsException : DomainExceptionBase
    {
        private localhostItemAlreadyExistsException(
            string message
        ) : base(message)
        {
        }

        public override int ExceptionCode { get; protected set; } = (int)Common.Exceptions.ExceptionCode.localhostItemAlreadyExists;


        public static void Raise(Guid categoryId, string localhostItemName, string message)
        {
            var exception = new localhostItemAlreadyExistsException(
                message ?? $"The item {localhostItemName} already exist in the category '{categoryId}'."
            );

            exception.Data["CategoryId"] = categoryId;
            exception.Data["localhostItemName"] = localhostItemName;

            throw exception;
        }

        public static void Raise(Guid categoryId, string localhostItemName)
        {
            Raise(categoryId, localhostItemName, null);
        }
    }
}
