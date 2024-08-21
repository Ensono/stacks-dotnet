using System;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;

public class SearchMenuResultItem
{
    public Guid? Id { get; private set; }

    public Guid RestaurantId { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public bool? Enabled { get; private set; }

    public static SearchMenuResultItem FromDomain(Menu menu)
    {
        return new SearchMenuResultItem()
        {
            Id = menu.Id,
            RestaurantId = menu.TenantId,
            Name = menu.Name,
            Description = menu.Description,
            Enabled = menu.Enabled
        };
    }
}
