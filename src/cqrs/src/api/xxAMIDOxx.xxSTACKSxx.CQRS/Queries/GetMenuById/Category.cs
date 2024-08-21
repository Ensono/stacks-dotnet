using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using Entity = xxAMIDOxx.xxSTACKSxx.Domain.Entities;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

public class Category
{
    [Required]
    public Guid Id { get; private set; }

    [Required]
    public string Name { get; private set; }

    public string Description { get; private set; }

    [Required]
    public List<MenuItem> Items { get; private set; }

    public static Category FromEntity(Entity.Category c)
    {
        return new Category()
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Items = c.Items?.Select(MenuItem.FromEntity).ToList()
        };
    }
}
