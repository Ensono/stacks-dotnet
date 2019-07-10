using System;
using System.Collections.Generic;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class MenuBuilder : IBuilder<Menu>
    {
        private Menu model;

        public MenuBuilder()
        {
            model = new Menu();
        }


        public MenuBuilder SetDefaultValues(string name)
        {
            var categoryBuilder = new CategoryBuilder();

            model.id = Guid.NewGuid().ToString();
            model.name = name;
            model.description = "Default test menu description";
            model.enabled = true;
            model.categories = new List<Category>()
            {
                categoryBuilder.SetDefaultValues("Burgers")
                .Build()
            };

            return this;
        }

        public MenuBuilder WithId(Guid id)
        {
            model.id = id.ToString();
            return this;
        }
        
        public MenuBuilder WithName(string name)
        {
            model.name = name;
            return this;
        }

        public MenuBuilder WithDescription(string description)
        {
            model.description = description;
            return this;
        }

        public MenuBuilder WithNoCategories()
        {
            model.categories = new List<Category>();
            return this;
        }

        public MenuBuilder WithCategories(List<Category> categories)
        {
            model.categories = categories;
            return this;
        }

        public MenuBuilder SetEnabled(bool enabled)
        {
            model.enabled = enabled;
            return this;
        }

        public Menu Build()
        {
            return model;
        }
    }
}
