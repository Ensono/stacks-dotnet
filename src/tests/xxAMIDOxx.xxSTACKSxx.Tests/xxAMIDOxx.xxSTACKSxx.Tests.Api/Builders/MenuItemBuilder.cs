using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class MenuItemBuilder : IBuilder<MenuItemRequest>
    {
        private MenuItemRequest menuItem;

        public MenuItemBuilder()
        {
            menuItem = new MenuItemRequest();
        }

        public MenuItemBuilder WithName(string name)
        {
            menuItem.name = name;
            return this;
        }

        public MenuItemBuilder WithDescription(string description)
        {
            menuItem.description = description;
            return this;
        }

        public MenuItemBuilder WithPrice(double price)
        {
            menuItem.price = price;
            return this;
        }

        public MenuItemBuilder SetAvailablity(bool available)
        {
            menuItem.available = available;
            return this;
        }

        public MenuItemRequest Build()
        {
            return menuItem;
        }
    }
}
