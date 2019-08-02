using System;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    public partial class CreateOrUpdateMenu
    {
        /// <example>Lunch Menu</example>
        [Required]
        public string Name { get; set; }

        /// <example>A delicious food selection for lunch. Available mon to fri from 11am to 4pm</example>
        [Required]
        public string Description { get; set; }

        [Required]
        public bool Enabled { get; set; }

        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        [Required]
        public Guid RestaurantId { get; set; }
    }
}
