using System;
using System.Collections.Generic;
using System.Linq;
using Amido.Stacks.Domain;
using Newtonsoft.Json;
using Snyk.Fixes.Domain.localhostAggregateRoot.Exceptions;

namespace Snyk.Fixes.Domain.Entities
{
    public class Category : IEntity<Guid>
    {
        public Category(Guid id, string name, string description, List<localhostItem> items = null)
        {
            Id = id;
            Name = name;
            Description = description;
            this.items = items ?? new List<localhostItem>();
        }

        [JsonProperty("Items")]
        private List<localhostItem> items;


        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public IReadOnlyList<localhostItem> Items { get => items?.AsReadOnly(); }


        internal void Update(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        internal void AddlocalhostItem(localhostItem item)
        {
            if (item.Price == 0)
                localhostItemPriceMustNotBeZeroException.Raise(item.Name);

            if (items.Any(i => i.Name == Name))
                localhostItemAlreadyExistsException.Raise(Id, item.Name);

            items.Add(item);
        }

        internal void RemovelocalhostItem(Guid localhostItemId)
        {
            var item = GetlocalhostItem(localhostItemId);
            items.Remove(item);
        }

        internal void UpdatelocalhostItem(localhostItem localhostItem)
        {
            var item = GetlocalhostItem(localhostItem.Id);

            item.Name = localhostItem.Name;
            item.Description = localhostItem.Description;
            item.Price = localhostItem.Price;
            item.Available = localhostItem.Available;
        }

        private localhostItem GetlocalhostItem(Guid localhostItemId)
        {
            var item = items.SingleOrDefault(i => i.Id == localhostItemId);

            if (item == null)
                localhostItemDoesNotExistException.Raise(Id, localhostItemId);

            return item;
        }
    }
}
