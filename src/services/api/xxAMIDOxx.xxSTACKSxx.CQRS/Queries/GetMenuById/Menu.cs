using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById
{
    public partial class Menu
    {
        [Required]
        public Guid Id { get; set; }

        public Guid RestaurantId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public List<Category> Categories { get; set; }

        [Required]
        public bool? Enabled { get; set; }
    }
}
