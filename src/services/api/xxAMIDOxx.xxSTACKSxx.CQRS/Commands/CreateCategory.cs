using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public partial class CreateCategory
    {
        /// <example>Cheese Burger</example>
        [Required]
        public string Name { get; set; }

        /// <example>A delicious patty with cheese</example>
        public string Description { get; set; }
    }
}
