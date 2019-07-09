using System;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.Models
{
    public partial class ResourceCreated
    {
        public ResourceCreated(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }
    }
}
