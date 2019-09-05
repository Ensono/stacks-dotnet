using System;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    /// <summary>
    /// Request model used by CreateMenu api endpoint
    /// </summary>
    public partial class CreateMenuRequest
    {
        /// <example>Name of menu created</example>
        [Required]
        public string Name { get; set; }

        /// <example>Description of menu created</example>
        [Required]
        public string Description { get; set; }

        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        [Required]
        public Guid TenantId { get; set; }
    }
}
