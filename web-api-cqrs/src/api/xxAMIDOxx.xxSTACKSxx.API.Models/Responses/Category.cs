using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Responses
{
    /// <summary>
    /// Response model used by GetById api endpoint
    /// </summary>
    public class Category
    {
        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        [Required]
        public Guid? Id { get; set; }

        /// <example>Name of category</example>
        [Required]
        public string Name { get; set; }

        /// <example>Description of category</example>
        public string Description { get; set; }

        /// <summary>
        /// Represents the items contained in the category
        /// </summary>
        [Required]
        public List<Item> Items { get; set; }
    }
}
