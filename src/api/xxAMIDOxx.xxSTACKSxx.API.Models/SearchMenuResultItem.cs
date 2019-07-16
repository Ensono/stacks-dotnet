using System;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    public partial class SearchMenuResultItem
    {
        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        public Guid Id { get; set; }

        /// <example>Lunch Menu</example>
        public string Name { get; set; }

        /// <example>A delicious food selection for lunch. Available mon to fri from 11am to 4pm</example>
        public string Description { get; set; }

        public bool Enabled { get; set; }
    }
}
