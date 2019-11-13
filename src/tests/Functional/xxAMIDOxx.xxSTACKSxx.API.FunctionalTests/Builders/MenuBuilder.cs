using System;
using System.Collections.Generic;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Models;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Builders
{
    public class MenuBuilder : IBuilder<Menu>
    {
        private Menu menu;

        public MenuBuilder()
        {
            menu = new Menu();
        }


        public MenuBuilder SetDefaultValues(string name)
        {
            var categoryBuilder = new CategoryBuilder();

            menu.id = Guid.NewGuid().ToString();
            menu.name = name;
            menu.description = "Default test menu description";
            menu.enabled = true;
            menu.categories = new List<Category>()
            {
                categoryBuilder.SetDefaultValues("Burgers")
                .Build()
            };

            return this;
        }

        public MenuBuilder WithId(Guid id)
        {
            menu.id = id.ToString();
            return this;
        }

        public MenuBuilder WithName(string name)
        {
            menu.name = name;
            return this;
        }

        public MenuBuilder WithDescription(string description)
        {
            menu.description = description;
            return this;
        }

        public MenuBuilder WithNoCategories()
        {
            menu.categories = new List<Category>();
            return this;
        }

        public MenuBuilder WithCategories(List<Category> categories)
        {
            menu.categories = categories;
            return this;
        }

        public MenuBuilder SetEnabled(bool enabled)
        {
            menu.enabled = enabled;
            return this;
        }

        public Menu Build()
        {
            return menu;
        }
    }
}
