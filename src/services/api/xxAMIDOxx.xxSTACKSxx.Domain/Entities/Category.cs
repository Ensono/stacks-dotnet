using System;
using System.Collections.Generic;
using System.Text;

namespace xxAMIDOxx.xxSTACKSxx.Domain.Entities
{
    public class Category
    {
        private List<MenuItem> items;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<MenuItem> Items { get => items?.AsReadOnly(); }
    }
}
