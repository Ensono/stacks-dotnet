using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById
{
    public class Menu
    {
        [Required]
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public List<Category> Categories { get; set; }

        [Required]
        public bool? Enabled { get; set; }
    }
}
