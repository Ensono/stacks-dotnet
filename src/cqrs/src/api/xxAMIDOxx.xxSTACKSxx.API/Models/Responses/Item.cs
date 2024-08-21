using System;
using System.ComponentModel.DataAnnotations;
using Query = xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

/// <summary>
/// Response model used by GetById api endpoint
/// </summary>
public class Item
{
    /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
    [Required]
    public Guid? Id { get; private set; }

    /// <example>Item name</example>
    [Required]
    public string Name { get; private set; }

    /// <example>Item description</example>
    public string Description { get; private set; }

    /// <example>1.50</example>
    [Required]
    public double? Price { get; private set; }

    /// <summary>
    /// Represents the status of the item. False if unavailable
    /// </summary>
    [Required]
    public bool? Available { get; private set; }

    public static Item FromQuery(Query.MenuItem item)
    {
        return new Item
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            Available = item.Available
        };
    }
}
