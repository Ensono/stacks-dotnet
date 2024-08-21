using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Query = xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

/// <summary>
/// Response model used by GetById api endpoint
/// </summary>
public class Menu
{
    /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
    [Required]
    public Guid? Id { get; private set; }

    /// <example>Menu name</example>
    [Required]
    public string Name { get; private set; }

    /// <example>Menu description</example>
    public string Description { get; private set; }

    /// <summary>
    /// Represents the categories contained in the menu
    /// </summary>
    public List<Category> Categories { get; private set; }

    /// <summary>
    /// Represents the status of the menu. False if disabled
    /// </summary>
    [Required]
    public bool? Enabled { get; private set; }

    public static Menu FromQuery(Query.Menu menu)
    {
        return new Menu
        {
            Id = menu.Id,
            Name = menu.Name,
            Description = menu.Description,
            Categories = menu.Categories?.Select(Category.FromQuery).ToList(),
            Enabled = menu.Enabled
        };
    }
}
