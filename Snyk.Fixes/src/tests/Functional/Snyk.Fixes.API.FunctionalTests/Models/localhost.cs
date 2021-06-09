using System.Collections.Generic;

namespace Snyk.Fixes.API.FunctionalTests.Models
{
    public class localhost
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<Category> categories { get; set; }
        public bool enabled { get; set; }
    }
}
