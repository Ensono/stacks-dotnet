using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amido.Stacks.Domain;
using xxAMIDOxx.xxSTACKSxx.Domain.Entities;
using xxAMIDOxx.xxSTACKSxx.Domain.Events;
using xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions;
using Amido.Stacks.Domain.Events;
using Amido.Stacks.DynamoDB.Converters;
using xxAMIDOxx.xxSTACKSxx.Domain.Converters;

namespace xxAMIDOxx.xxSTACKSxx.Domain;

#if (DynamoDb)
    public class Menu
    {
        [JsonProperty("Categories")]
        [DynamoDBIgnore]
        private List<Category> categories;

        // DynamoDB needs an empty constructor. In a real project you'll probably wrap your domain obj in a Dynamo DTO
        public Menu()
        {
            this.categories = categories ?? new List<Category>();
        }

        public Menu(Guid id, string name, Guid tenantId, string description, bool enabled, List<Category> categories = null)
        {
            Id = id;
            Name = name;
            TenantId = tenantId;
            Description = description;
            this.categories = categories ?? new List<Category>();
            Enabled = enabled;
        }

        [DynamoDBHashKey(typeof(DynamoDbGuidConverter))]
        public Guid Id { get; private set; }

        [DynamoDBProperty]
        public string Name { get; private set; }

        [DynamoDBProperty(typeof(DynamoDbGuidConverter))]
        public Guid TenantId { get; private set; }

        [DynamoDBProperty]
        public string Description { get; private set; }

        [JsonIgnore]
        [DynamoDBProperty(AttributeName = "Categories", Converter = typeof(DynamoDbCategoryConverter))]
        public IReadOnlyList<Category> Categories { get => categories?.AsReadOnly(); private set => categories = value.ToList(); }

        [DynamoDBProperty]
        public bool Enabled { get; private set; }

        public void Update(string name, string description, bool enabled)
        {
            this.Name = name;
            this.Description = description;
            this.Enabled = enabled;

            Emit(new MenuChanged());
        }

        public void AddCategory(Guid categoryId, string name, string description)
        {
            if (categories.Any(c => c.Name == name))
                CategoryAlreadyExistsException.Raise(Id, name);

            categories.Add(new Category(categoryId, name, description));

            Emit(new CategoryCreated());
        }

        public void UpdateCategory(Guid categoryId, string name, string description)
        {
            var category = GetCategory(categoryId);

            category.Update(name, description);

            Emit(new CategoryChanged());
        }

        public void RemoveCategory(Guid categoryId)
        {
            var category = GetCategory(categoryId);

            categories.Remove(category);

            Emit(new CategoryRemoved());
        }

        public void AddMenuItem(
            Guid categoryId,
            Guid menuItemId,
            string name,
            string description,
            double price,
            bool available)
        {
            var category = GetCategory(categoryId);

            category.AddMenuItem(new MenuItem(
                menuItemId,
                name,
                description,
                price,
                available));

            Emit(new MenuItemCreated());
        }

        public void UpdateMenuItem(
            Guid categoryId,
            Guid menuItemId,
            string name,
            string description,
            double price,
            bool available)
        {
            var category = GetCategory(categoryId);

            category.UpdateMenuItem(new MenuItem(
                menuItemId,
                name,
                description,
                price,
                available));

            Emit(new MenuItemChanged());
        }

        public void RemoveMenuItem(Guid categoryId, Guid menuItemId)
        {
            var category = GetCategory(categoryId);

            category.RemoveMenuItem(menuItemId);

            Emit(new MenuItemRemoved());
        }

        private Category GetCategory(Guid categoryId)
        {
            var category = categories.SingleOrDefault(c => c.Id == categoryId);

            if (category == null)
                CategoryDoesNotExistException.Raise(Id, categoryId);

            return category;
        }

        private void Emit(IDomainEvent domainEvent) { }
    }

#else

public class Menu : AggregateRoot<Guid>
{
    [JsonProperty("Categories")]
    private List<Category> categories;

    public Menu(Guid id, string name, Guid tenantId, string description, bool enabled, List<Category> categories = null)
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

        Emit(new MenuChanged());
    }

    public void AddCategory(Guid categoryId, string name, string description)
    {
        if (categories.Any(c => c.Name == name))
            CategoryAlreadyExistsException.Raise(Id, name);

        categories.Add(new Category(categoryId, name, description));

        Emit(new CategoryCreated());
    }

    public void UpdateCategory(Guid categoryId, string name, string description)
    {
        var category = GetCategory(categoryId);

        category.Update(name, description);

        Emit(new CategoryChanged());
    }

    public void RemoveCategory(Guid categoryId)
    {
        var category = GetCategory(categoryId);

        categories.Remove(category);

        Emit(new CategoryRemoved());
    }

    public void AddMenuItem(
        Guid categoryId,
        Guid menuItemId,
        string name,
        string description,
        double price,
        bool available)
    {
        var category = GetCategory(categoryId);

        category.AddMenuItem(
            new MenuItem(
                menuItemId,
                name,
                description,
                price,
                available));

        Emit(new MenuItemCreated());
    }

    public void UpdateMenuItem(
        Guid categoryId,
        Guid menuItemId,
        string name,
        string description,
        double price,
        bool available)
    {
        var category = GetCategory(categoryId);

        category.UpdateMenuItem(
            new MenuItem(
                menuItemId,
                name,
                description,
                price,
                available));

        Emit(new MenuItemChanged());
    }

    public void RemoveMenuItem(Guid categoryId, Guid menuItemId)
    {
        var category = GetCategory(categoryId);

        category.RemoveMenuItem(menuItemId);

        Emit(new MenuItemRemoved());
    }

    private Category GetCategory(Guid categoryId)
    {
        var category = categories.SingleOrDefault(c => c.Id == categoryId);

        if (category == null)
            CategoryDoesNotExistException.Raise(Id, categoryId);

        return category;
    }
}

#endif
