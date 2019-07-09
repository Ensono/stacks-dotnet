using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    public partial class CreateOrUpdateMenuItem
    {
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
