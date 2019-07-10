using System;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class ItemBuilder : IBuilder<Item>
    {
        private Item model;

        public ItemBuilder()
        {
            model = new Item();
        }

        public ItemBuilder SetDefaultValues(string name)
        {
            model.id = Guid.NewGuid().ToString();
            model.name = name;
            model.description = "Description added automatically in tests";
            model.available = true;
            model.price = "1.99";
            return this;
        }

        public ItemBuilder WithId(Guid id)
        {
            model.id = id.ToString();
            return this;
        }

        public ItemBuilder WithName(string name)
        {
            model.name = name;
            return this;
        }

        public ItemBuilder WithDescription(string description)
        {
            model.description = description;
            return this;
        }

        public ItemBuilder WithPrice(string price)
        {
            model.price = price;
            return this;
        }

        public Item Build()
        {
            return model;
        }
    }
}
