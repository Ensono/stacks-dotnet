using System;

namespace Snyk.Fixes.API.Models.Responses
{
    /// <summary>
    /// Response model representing a search result item in the Searchlocalhost api endpoint
    /// </summary>
    public class SearchlocalhostResultItem
    {
        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        public Guid Id { get; set; }

        /// <example>localhost name</example>
        public string Name { get; set; }

        /// <example>localhost description</example>
        public string Description { get; set; }

        /// <summary>
        /// Represents the status of the localhost. False if disabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}
