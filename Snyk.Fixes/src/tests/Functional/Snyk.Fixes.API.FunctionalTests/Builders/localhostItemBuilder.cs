using Snyk.Fixes.API.FunctionalTests.Models;

namespace Snyk.Fixes.API.FunctionalTests.Builders
{
    public class localhostItemBuilder : IBuilder<localhostItemRequest>
    {
        private readonly localhostItemRequest localhostItem;

        public localhostItemBuilder()
        {
            localhostItem = new localhostItemRequest();
        }

        public localhostItemBuilder WithName(string name)
        {
            localhostItem.name = name;
            return this;
        }

        public localhostItemBuilder WithDescription(string description)
        {
            localhostItem.description = description;
            return this;
        }

        public localhostItemBuilder WithPrice(double price)
        {
            localhostItem.price = price;
            return this;
        }

        public localhostItemBuilder WithAvailablity(bool available)
        {
            localhostItem.available = available;
            return this;
        }

        public localhostItemRequest Build()
        {
            return localhostItem;
        }
        
        public localhostItemBuilder SetDefaultValues(string name)
        {
            localhostItem.name = name;
            localhostItem.description = "Item description";
            localhostItem.price = 12.50;
            localhostItem.available = true;
            return this;
        }
    }
}
