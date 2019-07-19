using System;
using System.Collections.Generic;
using xxAMIDOxx.xxSTACKSxx.Domain.ValueObjects;

namespace xxAMIDOxx.xxSTACKSxx.Domain.Entities
{
    public class MenuItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }
        public Cuisine Cuisine { get; set; }
        public ItemClass Class { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
