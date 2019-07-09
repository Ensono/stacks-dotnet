using Amido.Stacks.Tests.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amido.Stacks.Tests.Api.Builders
{
    public class CategoryBuilder : IBuilder<Category>
    {
        private Category category;

        public CategoryBuilder()
        {
            category = new Category();
        }

        public CategoryBuilder CreateTestCategory(string name)
        {
            var itemBuilder = new ItemBuilder();

            category.id = Guid.NewGuid().ToString();
            category.name = name;
            category.items = new List<Item>()
            {
                itemBuilder.CreateTestItem("Burger")
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
