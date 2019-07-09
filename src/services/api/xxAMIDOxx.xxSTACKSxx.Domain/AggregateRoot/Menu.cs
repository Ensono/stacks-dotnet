using System;
using System.Collections.Generic;
using Amido.Stacks.Domain;
using xxAMIDOxx.xxSTACKSxx.Domain.Entities;

namespace xxAMIDOxx.xxSTACKSxx.Domain
{
    public class Menu : AggreagateRoot
    {
        private List<Category> categories;

        //TODO: set properties to private
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RestaurantId { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<Category> Categories { get => categories?.AsReadOnly(); }
        public bool? Enabled { get; set; }

        public void Update(string name, string description, bool enabled)
        {
            this.Name = name;
            this.Description = description;
            this.Enabled = Enabled;
        }
    }
}
