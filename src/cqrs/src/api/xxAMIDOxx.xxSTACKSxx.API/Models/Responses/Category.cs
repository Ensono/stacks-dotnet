using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Query = xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

/// <summary>
/// Response model used by GetById api endpoint
/// </summary>
public class Category
{
    /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
    [Required]
    public Guid? Id { get; private set; }

    /// <example>Name of category</example>
    [Required]
    public string Name { get; private set; }

    /// <example>Description of category</example>
    public string Description { get; private set; }

    /// <summary>
    /// Represents the items contained in the category
    /// </summary>
    [Required]
    public List<Item> Items { get; private set; }

    public static Category FromQuery(Query.Category category)
    {
        return new Category
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Items = category.Items?.Select(Item.FromQuery).ToList(),
        };
    }
}
