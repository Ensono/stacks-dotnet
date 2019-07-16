using System;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    public partial class MenuItem
    {
        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        [Required]
        public Guid? Id { get; set; }

        /// <example>CheeseBurguer</example>
        [Required]
        public string Name { get; set; }

        /// <example>A delicious patty covered with melted cheddar</example>
        public string Description { get; set; }

        /// <example>1.50</example>
        [Required]
        public double? Price { get; set; }

        [Required]
        public bool? Available { get; set; }
    }
}
