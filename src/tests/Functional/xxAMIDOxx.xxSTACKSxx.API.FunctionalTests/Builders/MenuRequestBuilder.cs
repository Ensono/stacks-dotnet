using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Models;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Builders
{
    public class MenuRequestBuilder : IBuilder<MenuRequest>
    {
        private MenuRequest menu;

        public MenuRequestBuilder()
        {
            menu = new MenuRequest();
        }

        public MenuRequestBuilder SetDefaultValues(string name)
        {
            menu.name = name;
            menu.description = "Updated menu description";
            menu.enabled = true;
            return this;
        }

        public MenuRequestBuilder WithName(string name)
        {
            menu.name = name;
            return this;
        }

        public MenuRequestBuilder WithDescription(string description)
        {
            menu.description = description;
            return this;
        }

        public MenuRequestBuilder SetEnabled(bool enabled)
        {
            menu.enabled = enabled;
            return this;
        }

        public MenuRequest Build()
        {
            return menu;
        }
    }
}
