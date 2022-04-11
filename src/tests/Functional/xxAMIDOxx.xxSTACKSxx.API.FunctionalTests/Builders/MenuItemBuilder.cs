using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Models;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Builders;

public class MenuItemBuilder : IBuilder<MenuItemRequest>
{
	private readonly MenuItemRequest menuItem;

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

	public MenuItemBuilder WithAvailablity(bool available)
	{
		menuItem.available = available;
		return this;
	}

	public MenuItemRequest Build()
	{
		return menuItem;
	}

	public MenuItemBuilder SetDefaultValues(string name)
	{
		menuItem.name = name;
		menuItem.description = "Item description";
		menuItem.price = 12.50;
		menuItem.available = true;
		return this;
	}
}
