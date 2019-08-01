using System;
using System.Collections.Generic;
using System.Linq;
using Amido.Stacks.Domain;
using xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions;

namespace xxAMIDOxx.xxSTACKSxx.Domain.Entities
{
    public class Category: IEntity<Guid>
    {
        private List<MenuItem> items = new List<MenuItem>();

        public Category(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
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
            item.Cuisine = menuItem.Cuisine;
            item.ItemClass = menuItem.ItemClass;
            item.Ingredients = menuItem.Ingredients;
        }

        private MenuItem GetMenuItem(Guid menuItemId)
        {
            var item = items.SingleOrDefault(i => i.Id == menuItemId);

            if (item == null)
                MenuItemDoesNotExistException.Raise(Id, menuItemId);

            return item;
        }
    }
}
