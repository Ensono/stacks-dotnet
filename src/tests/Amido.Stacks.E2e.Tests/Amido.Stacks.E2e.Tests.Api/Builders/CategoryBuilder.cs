using Amido.Stacks.E2e.Tests.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amido.Stacks.E2e.Tests.Api.Builders
{
    public class CategoryBuilder : IBuilder<Category>
    {
        private Category category;

        public CategoryBuilder()
        {
            category = new Category();
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
