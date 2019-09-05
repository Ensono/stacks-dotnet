﻿using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    /// <summary>
    /// Request model used by CreateCategory api endpoint
    /// </summary>
    public partial class CreateCategoryRequest
    {
        /// <example>Name of category created</example>
        [Required]
        public string Name { get; set; }

        /// <example>Description of category created</example>
        public string Description { get; set; }
    }
}
