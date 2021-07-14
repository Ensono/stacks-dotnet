using System;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Responses
{
    /// <summary>
    /// Response model returned by api endpoints when a new resource is created
    /// </summary>
    public class ResourceCreatedResponse
    {
        /// <param name="id">Id of newly created resource</param>
        public ResourceCreatedResponse(Guid id)
        {
            Id = id;
        }

        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        public Guid Id { get; set; }
    }
}
