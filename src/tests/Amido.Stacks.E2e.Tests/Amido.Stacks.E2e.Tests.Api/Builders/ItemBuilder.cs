using Amido.Stacks.E2e.Tests.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amido.Stacks.E2e.Tests.Api.Builders
{
    public class ItemBuilder : IBuilder<Item>
    {
        private Item item;

        public ItemBuilder()
        {
            item = new Item();
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
