using System;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById
{
    public class MenuItem
    {
        [Required]
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public bool Available { get; set; }
    }
}
