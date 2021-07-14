using System;
using System.Collections.Generic;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Models;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Builders
{
    public class CategoryBuilder : IBuilder<Category>
    {
        private Category category;

        public CategoryBuilder()
        {
            category = new Category();
        }

        public CategoryBuilder SetDefaultValues(string name)
        {
            var itemBuilder = new ItemBuilder();

            category.id = Guid.NewGuid().ToString();
            category.name = name;
            category.items = new List<Item>()
            {
                itemBuilder.SetDefaultValues("Burger")
                .Build()
            };
            return this;
        }

        public CategoryBuilder WithId(Guid id)
        {
            category.id = id.ToString();
            return this;
        }

        public CategoryBuilder WithName(string name)
        {
            category.name = name;
            return this;
        }
        public CategoryBuilder WithNoItems()
        {
            category.items = new List<Item>();
            return this;
        }

        public CategoryBuilder WithItems(List<Item> items)
        {
            category.items = items;
            return this;
        }

        public Category Build()
        {
            return category;
        }
    }
}
