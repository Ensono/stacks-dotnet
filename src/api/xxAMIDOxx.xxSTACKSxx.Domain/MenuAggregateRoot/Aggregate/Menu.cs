using System;
using System.Collections.Generic;
using Amido.Stacks.Domain;
using xxAMIDOxx.xxSTACKSxx.Domain.Entities;
using xxAMIDOxx.xxSTACKSxx.Domain.Events;
using xxAMIDOxx.xxSTACKSxx.Domain.ValueObjects;

namespace xxAMIDOxx.xxSTACKSxx.Domain
{
    public class Menu : AggregateRoot, IEntity<Guid>
    {
        private List<Category> categories;

        //TODO: set properties to private

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid RestaurantId { get; set; }

        public string Description { get; set; }

        public IReadOnlyList<Category> Categories { get => categories?.AsReadOnly(); }

        public bool Enabled { get; set; }

        public void Update(string name, string description, bool enabled)
        {
            this.Name = name;
            this.Description = description;
            this.Enabled = enabled;

            Emit(new MenuChanged());
        }

        public void AddCategory(string name, string description)
        {

        }

        public void UpdateCategory(string name, string description)
        {

        }

        public void RemoveCategory(Guid categoryId)
        {

        }

        public void AddMenuItem(Guid categoryId,
                                    Guid menuItemId,
                                    string Name,
                                    string Description,
                                    double Price,
                                    bool Available,
                                    Cuisine Cuisine,
                                    string Class,
                                    List<Ingredient> Ingredients)
        {

        }

        public void UpdateMenuItem(Guid categoryId,
                            Guid menuItemId,
                            string Name,
                            string Description,
                            double Price,
                            bool Available,
                            Cuisine Cuisine,
                            string Class,
                            List<Ingredient> Ingredients)
        {

        }

        public void RemoveMenuItem(Guid categoryId, Guid menuItemId)
        {

        }
    }
}
