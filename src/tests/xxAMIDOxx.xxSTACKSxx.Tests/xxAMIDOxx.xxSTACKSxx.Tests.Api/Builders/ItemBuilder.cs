using System;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class ItemBuilder : IBuilder<Item>
    {
        private Item item;

        public ItemBuilder()
        {
            item = new Item();
        }

        public ItemBuilder SetDefaultValues(string name)
        {
            item.id = Guid.NewGuid().ToString();
            item.name = name;
            item.description = "Description added automatically in tests";
            item.available = true;
            item.price = "1.99";
            return this;
        }

        public ItemBuilder WithId(Guid id)
        {
            item.id = id.ToString();
            return this;
        }

        public ItemBuilder WithName(string name)
        {
            item.name = name;
            return this;
        }

        public ItemBuilder WithDescription(string description)
        {
            item.description = description;
            return this;
        }

        public ItemBuilder WithPrice(string price)
        {
            item.price = price;
            return this;
        }

        public Item Build()
        {
            return item;
        }
    }
}
