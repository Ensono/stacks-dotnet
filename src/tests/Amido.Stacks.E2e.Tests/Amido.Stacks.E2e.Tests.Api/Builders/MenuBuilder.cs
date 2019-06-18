using Amido.Stacks.E2e.Tests.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amido.Stacks.E2e.Tests.Api.Builders
{
    public class MenuBuilder : IBuilder<Menu>
    {
        private Menu menu;

        public MenuBuilder()
        {
            menu = new Menu();
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
