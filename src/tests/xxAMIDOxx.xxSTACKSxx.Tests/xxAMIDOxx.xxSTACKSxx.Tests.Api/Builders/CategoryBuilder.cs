using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class CategoryBuilder : IBuilder<Category>
    {
        private Category model;

        public CategoryBuilder()
        {
            model = new Category();
        }

        public CategoryBuilder SetDefaultValues(string name)
        {
            var itemBuilder = new ItemBuilder();

            model.id = Guid.NewGuid().ToString();
            model.name = name;
            model.items = new List<Item>()
            {
                itemBuilder.SetDefaultValues("Burger")
                .Build()
            };
            return this;
        }

        public CategoryBuilder WithId(Guid id)
        {
            model.id = id.ToString();
            return this;
        }

        public CategoryBuilder WithName(string name)
        {
            model.name = name;
            return this;
        }
        public CategoryBuilder WithNoItems()
        {
            model.items = new List<Item>();
            return this;
        }

        public CategoryBuilder WithItems(List<Item> items)
        {
            model.items = items;
            return this;
        }

        public Category Build()
        {
            return model;
        }
    }
}
