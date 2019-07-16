using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    public partial class Category
    {
        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        [Required]
        public Guid? Id { get; set; }

        /// <example>Burguers</example>
        [Required]
        public string Name { get; set; }

        /// <example>A delicious selection of burguers</example>
        public string Description { get; set; }

        [Required]
        public List<MenuItem> Items { get; set; }
    }
}
