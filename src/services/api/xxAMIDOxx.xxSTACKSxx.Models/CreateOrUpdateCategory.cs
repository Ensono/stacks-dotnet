using System;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.Models
{
    public partial class CreateOrUpdateCategory
    { 
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
