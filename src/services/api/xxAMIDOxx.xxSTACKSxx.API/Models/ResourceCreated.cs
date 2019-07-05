using System;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    public partial class ResourceCreated
    {
        public ResourceCreated(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
