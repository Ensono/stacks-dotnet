using System;
using System.ComponentModel.DataAnnotations;

namespace xxAMIDOxx.xxSTACKSxx.Models
{ 
    public partial class SearchResultItem
    { 
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool? Enabled { get; set; }
    }
}
