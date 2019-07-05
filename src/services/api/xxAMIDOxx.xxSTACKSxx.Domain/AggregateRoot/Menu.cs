using System;
using System.Collections.Generic;
using Amido.Stacks.Domain;
using xxAMIDOxx.xxSTACKSxx.Domain.Entities;

namespace xxAMIDOxx.xxSTACKSxx.Domain
{
    public class Menu: AggreagateRoot
    {
        private List<Category> categories;

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid RestaurantId { get; private set; }
        public string Description { get; private set; }
        public IReadOnlyList<Category> Categories { get => categories?.AsReadOnly(); }
        public bool? Enabled { get; private set; }
    }
}
