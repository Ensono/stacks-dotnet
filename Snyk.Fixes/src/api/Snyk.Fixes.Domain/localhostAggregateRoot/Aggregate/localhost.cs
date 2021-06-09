using System;
using System.Collections.Generic;
using System.Linq;
using Amido.Stacks.Domain;
using Newtonsoft.Json;
using Snyk.Fixes.Domain.Entities;
using Snyk.Fixes.Domain.Events;
using Snyk.Fixes.Domain.localhostAggregateRoot.Exceptions;

namespace Snyk.Fixes.Domain
{
    public class localhost : AggregateRoot<Guid>
    {
        [JsonProperty("Categories")]
        private List<Category> categories;

        public localhost(Guid id, string name, Guid tenantId, string description, bool enabled, List<Category> categories = null)
        {
            Id = id;
            Name = name;
            TenantId = tenantId;
            Description = description;
            this.categories = categories ?? new List<Category>(); 
            Enabled = enabled;
        }

        public string Name { get; private set; }

        public Guid TenantId { get; private set; }

        public string Description { get; private set; }

        [JsonIgnore]
        public IReadOnlyList<Category> Categories { get => categories?.AsReadOnly(); private set => categories = value.ToList(); }

        public bool Enabled { get; private set; }

        public void Update(string name, string description, bool enabled)
        {
            this.Name = name;
            this.Description = description;
            this.Enabled = enabled;

            Emit(new localhostChanged());//TODO: Pass the event data
        }

        public void AddCategory(Guid categoryId, string name, string description)
        {
            if (categories.Any(c => c.Name == name))
                CategoryAlreadyExistsException.Raise(Id, name);

            categories.Add(new Category(categoryId, name, description));

            Emit(new CategoryCreated());//TODO: Pass the event data
        }

        public void UpdateCategory(Guid categoryId, string name, string description)
        {
            var category = GetCategory(categoryId);

            category.Update(name, description);

            Emit(new CategoryChanged());//TODO: Pass the event data
        }

        public void RemoveCategory(Guid categoryId)
        {
            var category = GetCategory(categoryId);

            categories.Remove(category);

            Emit(new CategoryRemoved());//TODO: Pass the event data
        }

        public void AddlocalhostItem(Guid categoryId,
                                    Guid localhostItemId,
                                    string name,
                                    string description,
                                    double price,
                                    bool available
            )
        {
            var category = GetCategory(categoryId);

            category.AddlocalhostItem(
                    new localhostItem(
                        localhostItemId,
                        name,
                        description,
                        price,
                        available
                        )
                );

            Emit(new localhostItemCreated());//TODO: Pass the event data
        }

        public void UpdatelocalhostItem(Guid categoryId,
                            Guid localhostItemId,
                            string name,
                            string description,
                            double price,
                            bool available
            )
        {
            var category = GetCategory(categoryId);

            category.UpdatelocalhostItem(
                    new localhostItem(
                        localhostItemId,
                        name,
                        description,
                        price,
                        available
                     )
            );

            Emit(new localhostItemChanged());//TODO: Pass the event data
        }

        public void RemovelocalhostItem(Guid categoryId, Guid localhostItemId)
        {
            var category = GetCategory(categoryId);

            category.RemovelocalhostItem(localhostItemId);

            Emit(new localhostItemRemoved());//TODO: Pass the event data
        }

        private Category GetCategory(Guid categoryId)
        {
            var category = categories.SingleOrDefault(c => c.Id == categoryId);

            if (category == null)
                CategoryDoesNotExistException.Raise(Id, categoryId);

            return category;
        }

    }
}
