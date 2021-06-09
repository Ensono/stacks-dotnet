using System;
using Amido.Stacks.Core.Exceptions;

namespace Snyk.Fixes.Domain.localhostAggregateRoot.Exceptions
{
    [Serializable]
    public class localhostItemPriceMustNotBeZeroException : DomainExceptionBase
    {
        private localhostItemPriceMustNotBeZeroException(
            string message
        ) : base(message)
        {
        }

        public override int ExceptionCode { get; protected set; } = (int)Common.Exceptions.ExceptionCode.localhostItemPriceMustNotBeZero;

        public static void Raise(string itemName, string message)
        {
            var exception = new localhostItemPriceMustNotBeZeroException(
                message ?? $"The price for the item {itemName} had price as zero. A price must be provided."
            );

            exception.Data["ItemName"] = itemName;
            throw exception;
        }

        public static void Raise(string itemName)
        {
            Raise(itemName, null);
        }
    }
}
