using Amido.Stacks.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions
{
    public class MenuItemPriceMustNotBeZeroException : DomainException
    {
        private MenuItemPriceMustNotBeZeroException(
            string message
        ) : base(message)
        {
        }

        public override int ExceptionCode => (int)Common.Exceptions.ExceptionCode.MenuItemPriceMustNotBeZero;

        public static void Raise(string itemName, string message)
        {
            var exception = new MenuItemPriceMustNotBeZeroException(
                message ?? $"The price for the item {itemName} had price as zero. A price must be provided."
            );

            exception.Data["ItemName"] = itemName;
            throw exception;
        }

        public static void Raise(string itemName)
        {
            Raise(itemName);
        }
    }
}
