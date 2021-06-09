using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Snyk.Fixes.CQRS.Queries.GetlocalhostById
{
    public class Category
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public List<localhostItem> Items { get; set; }
    }
}
