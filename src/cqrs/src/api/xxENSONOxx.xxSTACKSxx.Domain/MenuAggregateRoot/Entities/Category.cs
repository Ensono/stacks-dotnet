using System;
using System.Collections.Generic;
using System.Linq;
using xxENSONOxx.xxSTACKSxx.Shared.Domain;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions;

namespace xxENSONOxx.xxSTACKSxx.Domain.Entities;

public class Category(Guid id, string name, string description, List<MenuItem> items = null)
    : IEntity<Guid>
{
    [JsonProperty("Items")]
    private List<MenuItem> items = items ?? new List<MenuItem>();


    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;

    [JsonIgnore]
    public IReadOnlyList<MenuItem> Items { get => items?.AsReadOnly(); }


    internal void Update(string name, string description)
    {
        this.Name = name;
        this.Description = description;
    }

    internal void AddMenuItem(MenuItem item)
    {
        if (item.Price == 0)
            MenuItemPriceMustNotBeZeroException.Raise(item.Name);

        if (items.Any(i => i.Name == Name))
            MenuItemAlreadyExistsException.Raise(Id, item.Name);

        items.Add(item);
    }

    internal void RemoveMenuItem(Guid menuItemId)
    {
        var item = GetMenuItem(menuItemId);
        items.Remove(item);
    }

    internal void UpdateMenuItem(MenuItem menuItem)
    {
        var item = GetMenuItem(menuItem.Id);

        item.Name = menuItem.Name;
        item.Description = menuItem.Description;
        item.Price = menuItem.Price;
        item.Available = menuItem.Available;
    }

    private MenuItem GetMenuItem(Guid menuItemId)
    {
        var item = items.SingleOrDefault(i => i.Id == menuItemId);

        if (item == null)
            MenuItemDoesNotExistException.Raise(Id, menuItemId);

        return item;
    }
}
