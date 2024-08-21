using System;
using System.ComponentModel.DataAnnotations;
using Entity = xxAMIDOxx.xxSTACKSxx.Domain.Entities;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

public class MenuItem
{
    [Required]
    public Guid? Id { get; private set; }

    [Required]
    public string Name { get; private set; }

    public string Description { get; private set; }

    [Required]
    public double Price { get; private set; }

    [Required]
    public bool Available { get; private set; }

    public static MenuItem FromEntity(Entity.MenuItem i)
    {
        return new MenuItem()
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description,
            Price = i.Price,
            Available = i.Available
        };
    }
}
