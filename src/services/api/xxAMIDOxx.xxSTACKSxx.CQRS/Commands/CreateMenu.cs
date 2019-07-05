using System;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public partial class CreateMenu
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool? Enabled { get; set; }
    }
}
