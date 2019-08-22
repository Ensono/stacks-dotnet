using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    public partial class CreateOrUpdateCategory
    {
        /// <example>Burguers</example>
        [Required]
        public string Name { get; set; }

        /// <example>A delicious selection of burguers</example>
        public string Description { get; set; }
    }
}
