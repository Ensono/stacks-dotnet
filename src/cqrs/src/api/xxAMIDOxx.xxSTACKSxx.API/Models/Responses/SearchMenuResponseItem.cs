using System;
using Query = xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

/// <summary>
/// Response model representing a search result item in the SearchMenu api endpoint
/// </summary>
public class SearchMenuResponseItem
{
    /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
    public Guid Id { get; private set; }

    /// <example>Menu name</example>
    public string Name { get; private set; }

    /// <example>Menu description</example>
    public string Description { get; private set; }

    /// <summary>
    /// Represents the status of the menu. False if disabled
    /// </summary>
    public bool Enabled { get; private set; }

    public static SearchMenuResponseItem FromSearchMenuResultItem(Query.SearchMenuResultItem item)
    {
        return new SearchMenuResponseItem()
        {
            Id = item.Id ?? Guid.Empty,
            Name = item.Name,
            Description = item.Description,
            Enabled = item.Enabled ?? false
        };
    }
}
