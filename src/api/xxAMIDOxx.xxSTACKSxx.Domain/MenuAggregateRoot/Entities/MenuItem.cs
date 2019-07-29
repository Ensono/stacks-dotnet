﻿using System;
using System.Collections.Generic;
using xxAMIDOxx.xxSTACKSxx.Domain.ValueObjects;

namespace xxAMIDOxx.xxSTACKSxx.Domain.Entities
{
    public class MenuItem
    {
        public MenuItem(Guid id, string name, string description, double price, bool available, string itemClass, Cuisine cuisine, List<Ingredient> ingredients)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Available = available;
            ItemClass = itemClass;
            Cuisine = cuisine;
            Ingredients = ingredients;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool Available { get; set; }

        public string ItemClass { get; set; }

        public Cuisine Cuisine { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}
