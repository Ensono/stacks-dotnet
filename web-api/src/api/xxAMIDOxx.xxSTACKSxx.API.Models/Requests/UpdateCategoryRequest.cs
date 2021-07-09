using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Requests
{
    /// <summary>
    /// Request model used by UpdateCategory api endpoint
    /// </summary>
    public class UpdateCategoryRequest
    {
        /// <example>Burguers</example>
        [Required]
        public string Name { get; set; }

        /// <example>A delicious selection of burguers</example>
        public string Description { get; set; }
    }
}
