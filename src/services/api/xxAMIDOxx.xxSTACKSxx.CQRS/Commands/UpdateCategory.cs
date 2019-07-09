using System;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public partial class UpdateCategory
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
