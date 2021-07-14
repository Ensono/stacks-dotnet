using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Responses
{
    /// <summary>
    /// Response model used by GetById api endpoint
    /// </summary>
    public class Menu
    {
        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        [Required]
        public Guid? Id { get; set; }

        /// <example>Menu name</example>
        [Required]
        public string Name { get; set; }

        /// <example>Menu description</example>
        public string Description { get; set; }

        /// <summary>
        /// Represents the categories contained in the menu
        /// </summary>
        public List<Category> Categories { get; set; }

        /// <summary>
        /// Represents the status of the menu. False if disabled
        /// </summary>
        [Required]
        public bool? Enabled { get; set; }
    }
}
