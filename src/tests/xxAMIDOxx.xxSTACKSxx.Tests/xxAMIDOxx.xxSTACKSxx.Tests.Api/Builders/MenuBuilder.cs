using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class MenuBuilder : IBuilder<Menu>
    {
        private Menu menu;

        public MenuBuilder()
        {
            menu = new Menu();
        }


        public MenuBuilder CreateTestMenu(string name)
        {
            menu.name = name;
            menu.description = "Default test menu description";
            menu.enabled = true;

            return this;
        }
        
        public MenuBuilder WithName(string name)
        {
            menu.name = name;
            return this;
        }

        public MenuBuilder WithDescription(string descripition)
        {
            menu.description = descripition;
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
