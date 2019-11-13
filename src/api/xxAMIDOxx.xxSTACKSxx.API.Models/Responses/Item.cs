using System;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Responses
{
    /// <summary>
    /// Response model used by GetById api endpoint
    /// </summary>
    public class Item
    {
        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        [Required]
        public Guid? Id { get; set; }

        /// <example>Item name</example>
        [Required]
        public string Name { get; set; }

        /// <example>Item description</example>
        public string Description { get; set; }

        /// <example>1.50</example>
        [Required]
        public double? Price { get; set; }

        /// <summary>
        /// Represents the status of the item. False if unavailable
        /// </summary>
        [Required]
        public bool? Available { get; set; }
    }
}
