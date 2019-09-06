using System;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    /// <summary>
    /// Response model returned by api endpoints when a new resource is created
    /// </summary>
    public partial class ResourceCreatedResult
    {
        /// <param name="id">Id of newly created resource</param>
        public ResourceCreatedResult(Guid id)
        {
            Id = id;
        }

        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        public Guid Id { get; set; }
    }
}
