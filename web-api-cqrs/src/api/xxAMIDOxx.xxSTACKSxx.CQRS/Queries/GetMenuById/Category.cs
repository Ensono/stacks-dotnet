using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById
{
    public class Category
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public List<MenuItem> Items { get; set; }
    }
}
