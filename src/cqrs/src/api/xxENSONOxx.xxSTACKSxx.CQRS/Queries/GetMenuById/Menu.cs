using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Domain = xxENSONOxx.xxSTACKSxx.Domain;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

public class Menu
{
    [Required]
    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    [Required]
    public string Name { get; private set; }

    public string Description { get; private set; }

    [Required]
    public List<Category> Categories { get; private set; }

    [Required]
    public bool? Enabled { get; private set; }

    public static Menu FromDomain(Domain.Menu menu)
    {
        return new Menu()
        {
            Id = menu.Id,
            TenantId = menu.TenantId,
            Name = menu.Name,
            Description = menu.Description,
            Enabled = menu.Enabled,
            Categories = menu.Categories?.Select(Category.FromEntity).ToList()
        };
    }
}
