using System;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    public partial class ResourceCreated
    {
        public ResourceCreated(Guid id)
        {
            Id = id;
        }

        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        public Guid Id { get; set; }
    }
}
